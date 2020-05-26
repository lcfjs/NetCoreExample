using Example.UnitOfWork.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Example.UnitOfWork
{
    public interface IUnitOfWork<TContext> where TContext : DbContext
    {
        DbContext DbContext { get; }

        /// <summary>
        /// 获取通用仓储
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        IRepository<TEntity> GetRepository<TEntity>() where TEntity : class;

        /// <summary>
        /// 获取自定义仓储，仓储需要进行依赖注入
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T GetCustomRepository<T>() where T : class;


        int SaveChanges();
        Task<int> SaveChangesAsync();

        /// <summary>
        /// 执行SQL
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        int ExecuteSqlCommand(string sql, params object[] parameters);

        /// <summary>
        /// 执行SQL
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<int> ExecuteSqlCommandAsync(string sql, params object[] parameters);

        /// <summary>
        /// 执行SQL语句，TEntity需要在TContext定义属性
        /// </summary>
        /// <typeparam name="TEntity">需要在TContext定义属性</typeparam>
        /// <param name="sql">SQL</param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        IQueryable<TEntity> FromSql<TEntity>(string sql, params object[] parameters) where TEntity : class;

        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <typeparam name="TElement"></typeparam>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        List<T> SqlQuery<T>(string sql, params object[] parameters) where T : new();

        /// <summary>
        /// 执行SQL语句返回分页
        /// </summary>
        /// <typeparam name="TElement"></typeparam>
        /// <param name="sql"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        IPageList<T> SqlQuery<T>(string sql, int pageIndex, int pageSize, params object[] parameters) where T : new();

        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        DataTable SqlQuery(string sql, params object[] parameters);

        /// <summary>
        /// 执行SQL语句（分页）
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        DataTable SqlQuery(string sql, int pageIndex, int pageSize, out int totalCount, params object[] parameters);
    }
}
