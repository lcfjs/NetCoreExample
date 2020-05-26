using Example.UnitOfWork.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Example.Core.Extension;

namespace Example.UnitOfWork
{
    public class UnitOfWork<TContext> : IUnitOfWork<TContext> where TContext : DbContext
    {
        private readonly TContext _context;
        private Dictionary<Type, object> repositories;

        public UnitOfWork(TContext context)
        {
            _context = context;
        }

        public DbContext DbContext => _context;

        public IRepository<TEntity> GetRepository<TEntity>() where TEntity : class
        {
            if (repositories == null)
            {
                repositories = new Dictionary<Type, object>();
            }
            var type = typeof(TEntity);
            if (!repositories.ContainsKey(type))
            {
                repositories[type] = new Repository<TEntity>(_context);
            }
            return (IRepository<TEntity>)repositories[type];
        }

        public T GetCustomRepository<T>() where T : class
        {
            if (repositories == null)
            {
                repositories = new Dictionary<Type, object>();
            }
            var type = typeof(T);
            if (!repositories.ContainsKey(type))
            {
                var temp = _context.GetService<T>();
                repositories[type] = temp ?? throw new Exception($"请先注入{type.Name}");
            }
            return (T)repositories[type];
        }

        public int ExecuteSqlCommand(string sql, params object[] parameters)
        {
            return _context.Database.ExecuteSqlRaw(sql, parameters);
        }

        public async Task<int> ExecuteSqlCommandAsync(string sql, params object[] parameters)
        {
            return await _context.Database.ExecuteSqlRawAsync(sql, parameters);
        }

        public IQueryable<TEntity> FromSql<TEntity>(string sql, params object[] parameters) where TEntity : class
        {
            return _context.Set<TEntity>().FromSqlRaw(sql, parameters);
        }

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public List<T> SqlQuery<T>(string sql, params object[] parameters) where T : new()
        {
            DbConnection conn = null;
            DbCommand cmd = null;
            DbDataReader reader = null;
            try
            {
                conn = GetConnection();
                cmd = GetCommand(conn, parameters);
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.Text;
                reader = cmd.ExecuteReader();
                DataTable table = new DataTable();
                table.Load(reader);

                return table.ToList<T>();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null && !reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
                if (cmd != null)
                {
                    cmd.Parameters.Clear();
                }
                if (conn != null && conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }

        public IPageList<T> SqlQuery<T>(string sql, int pageIndex, int pageSize, params object[] parameters) where T : new()
        {
            var table = SqlQuery(sql, pageIndex, pageSize, out int totalCount, parameters);
            if (totalCount == 0)
            {
                return null;
            }
            return new PageList<T>(table.ToList<T>(), pageIndex, pageSize, totalCount);
        }

        public DataTable SqlQuery(string sql, params object[] parameters)
        {
            DbConnection conn = null;
            DbCommand cmd = null;
            DbDataReader reader = null;
            try
            {
                conn = GetConnection();
                cmd = GetCommand(conn, parameters);
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.Text;
                reader = cmd.ExecuteReader();
                DataTable table = new DataTable();
                table.Load(reader);

                return table;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null && !reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
                if (cmd != null)
                {
                    cmd.Parameters.Clear();
                }
                if (conn != null && conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }

        public DataTable SqlQuery(string sql, int pageIndex, int pageSize, out int totalCount, params object[] parameters)
        {
            if (pageIndex < 0)
            {
                pageIndex = 0;
            }
            DbConnection conn = null;
            DbCommand cmd = null;
            DbDataReader reader = null;
            try
            {
                conn = GetConnection();
                cmd = GetCommand(conn, parameters);
                cmd.CommandType = CommandType.Text;

                totalCount = GetPageCount(conn, cmd, sql);
                if (totalCount == 0)
                {
                    return null;
                }

                #region 处理SQL 支持普通和CTE

                // 去掉分号
                sql = sql.Replace(";", " ");

                // 取出CTE表达式
                string sqlCTE = "";
                Regex regxWith = new Regex("with[\\s\\S]*\\)[\\s]*?(?=select)", RegexOptions.IgnoreCase);
                MatchCollection mcWith = regxWith.Matches(sql);
                if (mcWith != null && mcWith.Count > 0)
                {
                    sqlCTE += (mcWith[0].Groups[0].Value + " ");
                    sql = sql.Replace(mcWith[0].Groups[0].Value, " ");
                }

                // 取出order by
                string sqlOrderBy = "";
                Regex regxOrderBy = new Regex("order\\s*by[\\s\\S]*", RegexOptions.IgnoreCase);
                MatchCollection mcOrderBy = regxOrderBy.Matches(sql);
                if (mcOrderBy != null && mcOrderBy.Count > 0)
                {
                    sqlOrderBy += (mcOrderBy[0].Groups[0].Value + " ");
                    sql = sql.Replace(mcOrderBy[0].Groups[0].Value, " ");
                }
                if (string.IsNullOrWhiteSpace(sqlOrderBy))
                {
                    sqlOrderBy = "order by CreateTime desc";
                }

                string sqlText = string.Format("{0} {1} {2} limit {3},{4}", sqlCTE, sql, sqlOrderBy, pageSize * pageIndex, pageSize);

                #endregion 处理SQL 支持普通和CTE

                cmd.CommandText = sqlText;
                reader = cmd.ExecuteReader();
                DataTable table = new DataTable();
                table.Load(reader);
                return table;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null && !reader.IsClosed)
                {
                    reader.Close();
                    reader.Dispose();
                }
                if (cmd != null)
                {
                    cmd.Parameters.Clear();
                }
                if (conn != null && conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }

        public async Task Transaction(Action ac)
        {
            await Task.Run(() =>
            {
                using (var tran = _context.Database.BeginTransaction())
                {
                    try
                    {
                        ac();
                        tran.Commit();
                    }
                    catch (Exception)
                    {
                        tran.Rollback();
                        throw;
                    }
                }
            });
        }






        #region 私有方法
        private DbConnection GetConnection()
        {
            var conn = _context.Database.GetDbConnection();
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }
            return conn;
        }
        private DbCommand GetCommand(DbConnection conn, object[] parameters)
        {
            var cmd = conn.CreateCommand();

            if (parameters != null)
            {
                cmd.Parameters.AddRange(parameters);
            }

            return cmd;
        }
        private int GetPageCount(DbConnection conn, DbCommand cmd, string sql)
        {
            #region 处理SQL 支持普通和CTE

            // 去掉分号
            sql = sql.Replace(";", " ");

            // 取出CTE表达式
            string sqlCTE = "";
            Regex regxWith = new Regex("with[\\s\\S]*\\)[\\s]*?(?=select)", RegexOptions.IgnoreCase);
            MatchCollection mcWith = regxWith.Matches(sql);
            if (mcWith != null && mcWith.Count > 0)
            {
                sqlCTE += (mcWith[0].Groups[0].Value + " ");
                sql = sql.Replace(mcWith[0].Groups[0].Value, " ");
            }

            //替换列
            //cmdText = Regex.Replace(cmdText, "select[\\s\\S]*from", "select count(1) from", RegexOptions.IgnoreCase);
            Regex columnRegex = new Regex("select[\\s\\S]*?from", RegexOptions.IgnoreCase);
            MatchCollection columnMc = columnRegex.Matches(sql);
            if (columnMc != null && columnMc.Count > 0)
            {
                sql = sql.Replace(columnMc[0].Groups[0].Value, "select count(1) from");
            }

            // 去掉order by
            Regex regxOrderBy = new Regex("order\\s*by[\\s\\S]*", RegexOptions.IgnoreCase);
            MatchCollection mcOrderBy = regxOrderBy.Matches(sql);
            if (mcOrderBy != null && mcOrderBy.Count > 0)
            {
                sql = sql.Replace(mcOrderBy[0].Groups[0].Value, " ");
            }

            //组装SQL
            string sqlCount = string.Format("{0} {1}", sqlCTE, sql);

            #endregion 处理SQL 支持普通和CTE

            cmd.CommandText = sqlCount;
            object val = cmd.ExecuteScalar();

            return Convert.ToInt32(val);
        }
        #endregion
    }
}
