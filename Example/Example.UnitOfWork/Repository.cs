using Example.Model;
using Example.Model.Table.Sys;
using Example.UnitOfWork.Entity;
using Example.WebCore.BaseModel;
using Example.Core.Extension;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Example.UnitOfWork
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly DbContext _dbContext;
        protected readonly DbSet<TEntity> _dbSet;

        public Repository(DbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _dbSet = _dbContext.Set<TEntity>();
        }

        public virtual void Insert(TEntity entity)
        {
            _dbSet.Add(entity);
        }

        public virtual void Insert(TEntity entity, CurrentUserBase userInfo)
        {
            DetectBaseEntity(entity, userInfo, true);
            _dbSet.Add(entity);
        }

        public virtual void Insert(IEnumerable<TEntity> entities)
        {
            _dbSet.AddRange(entities);
        }

        public virtual void Insert(IEnumerable<TEntity> entities, CurrentUserBase userInfo)
        {
            DetectBaseEntity(entities, userInfo, true);
            _dbSet.AddRange(entities);
        }

        public virtual void Update(TEntity entity)
        {
            _dbSet.Update(entity);
        }

        public virtual void Update(TEntity entity, CurrentUserBase userInfo)
        {
            DetectBaseEntity(entity, userInfo, false);
            _dbSet.Update(entity);
        }

        public virtual void Update(IEnumerable<TEntity> entities, CurrentUserBase userInfo)
        {
            DetectBaseEntity(entities, userInfo, false);
            _dbSet.UpdateRange(entities);
        }
        public virtual void Delete(TEntity entity)
        {
            _dbSet.Remove(entity);
        }

        public virtual void Delete(IEnumerable<TEntity> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        public virtual void Delete(Expression<Func<TEntity, bool>> predicate)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException();
            }
            var list = _dbSet.Where(predicate).ToList();
            if (list.Count > 0)
            {
                _dbSet.RemoveRange(list);
            }
        }

        public virtual void LogicDelete(TEntity entity, CurrentUserBase userInfo)
        {
            if (!(entity is BaseEntity))
            {
                throw new Exception("逻辑删除只支持BaseEntity派生的实体");
            }
            var tmp = entity as BaseEntity;
            tmp.IsDelete = true;
            tmp.UpdateTime = DateTime.Now;
            tmp.UpdateUserId = userInfo.UserId;
            tmp.UpdateUserName = userInfo.UserName;
            _dbSet.Update(entity);
        }

        public virtual void LogicDelete(IEnumerable<TEntity> entities, CurrentUserBase userInfo)
        {
            foreach (var item in entities)
            {
                if (!(item is BaseEntity))
                {
                    throw new Exception("逻辑删除只支持BaseEntity派生的实体");
                }
                var tmp = item as BaseEntity;
                tmp.IsDelete = true;
                tmp.UpdateTime = DateTime.Now;
                tmp.UpdateUserId = userInfo.UserId;
                tmp.UpdateUserName = userInfo.UserName;
            }
            _dbSet.UpdateRange(entities);
        }

        public virtual void LogicDelete(Expression<Func<TEntity, bool>> predicate, CurrentUserBase userInfo)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException();
            }
            var list = _dbSet.Where(predicate).ToList();
            if (list.Count > 0)
            {
                foreach (var item in list)
                {
                    if (!(item is BaseEntity))
                    {
                        throw new Exception("逻辑删除只支持BaseEntity派生的实体");
                    }
                    var tmp = item as BaseEntity;
                    tmp.IsDelete = true;
                    tmp.UpdateTime = DateTime.Now;
                    tmp.UpdateUserId = userInfo.UserId;
                    tmp.UpdateUserName = userInfo.UserName;
                }
                _dbSet.UpdateRange(list);
            }
        }



        public virtual IQueryable<TEntity> FromSql(string sql, params object[] parameters)
        {
            return _dbSet.FromSqlRaw(sql, parameters); //_dbSet.FromSql(sql, parameters); 
        }




        public virtual TEntity GetFirstOrDefault(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null)
        {
            IQueryable<TEntity> query = _dbSet.AsNoTracking();
            if (predicate != null)
            {
                query = query.Where(predicate);
            }
            if (orderBy != null)
            {
                query = orderBy(query);
            }
            return query.FirstOrDefault();
        }

        public virtual TResult GetFirstOrDefault<TResult>(Expression<Func<TEntity, TResult>> selector, Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null)
        {
            IQueryable<TEntity> query = _dbSet.AsNoTracking();
            if (predicate != null)
            {
                query = query.Where(predicate);
            }
            if (orderBy != null)
            {
                query = orderBy(query);
            }
            return query.Select(selector).FirstOrDefault();
        }

        public virtual async Task<TEntity> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null)
        {
            IQueryable<TEntity> query = _dbSet.AsNoTracking();
            if (predicate != null)
            {
                query = query.Where(predicate);
            }
            if (orderBy != null)
            {
                query = orderBy(query);
            }
            return await query.FirstOrDefaultAsync();
        }

        public virtual async Task<TResult> GetFirstOrDefaultAsync<TResult>(Expression<Func<TEntity, TResult>> selector, Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null)
        {
            IQueryable<TEntity> query = _dbSet.AsNoTracking();
            if (predicate != null)
            {
                query = query.Where(predicate);
            }
            if (orderBy != null)
            {
                query = orderBy(query);
            }
            return await query.Select(selector).FirstOrDefaultAsync();
        }



        public virtual IEnumerable<TEntity> GetList(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null)
        {
            IQueryable<TEntity> query = _dbSet.AsNoTracking();
            if (predicate != null)
            {
                query = query.Where(predicate);
            }
            if (orderBy != null)
            {
                query = orderBy(query);
            }
            return query.ToList();
        }

        public virtual async Task<IEnumerable<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null)
        {
            IQueryable<TEntity> query = _dbSet.AsNoTracking();
            if (predicate != null)
            {
                query = query.Where(predicate);
            }
            if (orderBy != null)
            {
                query = orderBy(query);
            }
            return await query.ToListAsync();
        }

        public virtual IEnumerable<TResult> GetList<TResult>(Expression<Func<TEntity, TResult>> selector, Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null)
        {
            IQueryable<TEntity> query = _dbSet.AsNoTracking();
            if (predicate != null)
            {
                query = query.Where(predicate);
            }
            if (orderBy != null)
            {
                query = orderBy(query);
            }
            return query.Select(selector).ToList();
        }

        public virtual async Task<IEnumerable<TResult>> GetListAsync<TResult>(Expression<Func<TEntity, TResult>> selector, Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null)
        {
            IQueryable<TEntity> query = _dbSet.AsNoTracking();
            if (predicate != null)
            {
                query = query.Where(predicate);
            }
            if (orderBy != null)
            {
                query = orderBy(query);
            }
            return await query.Select(selector).ToListAsync();
        }



        public virtual async Task<IPageList<TEntity>> GetPageListAsync(Expression<Func<TEntity, bool>> predicate, int pageIndex, int pageSize, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null)
        {
            IQueryable<TEntity> query = _dbSet.AsNoTracking();
            if (predicate != null)
            {
                query = query.Where(predicate);
            }
            if (orderBy != null)
            {
                query = orderBy(query);
            }
            return await query.ToPageListAsync(pageIndex, pageSize);
        }

        public virtual async Task<IPageList<TResult>> GetPageListAsync<TResult>(Expression<Func<TEntity, bool>> predicate, int pageIndex, int pageSize, Func<IEnumerable<TEntity>, IEnumerable<TResult>> fun, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null)
        {
            IQueryable<TEntity> query = _dbSet.AsNoTracking();
            if (predicate != null)
            {
                query = query.Where(predicate);
            }
            if (orderBy != null)
            {
                query = orderBy(query);
            }
            return await query.ToPageListAsync(fun, pageIndex, pageSize);
        }





        #region 私有函数

        private void DetectBaseEntity(IEnumerable<TEntity> entities, CurrentUserBase userInfo, bool isAdd = true)
        {
            foreach (var item in entities)
            {
                DetectBaseEntity(item, userInfo, isAdd);
            }
        }

        private void DetectBaseEntity(TEntity entity, CurrentUserBase userInfo, bool isAdd = true)
        {
            if (entity is BaseEntity)
            {
                var temp = entity as BaseEntity;
                if (string.IsNullOrWhiteSpace(temp.Id))
                {
                    temp.Id = Guid.NewGuid().ToString();
                }
                if (isAdd)
                {
                    temp.CreateTime = DateTime.Now;
                    temp.CreateUserId = userInfo.UserId;
                    temp.CreateUserName = userInfo.UserName;
                    temp.Sort = DateTime.Now.ToTimestampMillisecond();
                }
                else
                {
                    temp.UpdateTime = DateTime.Now;
                    temp.UpdateUserId = userInfo.UserId;
                    temp.UpdateUserName = userInfo.UserName;
                }
            }
        }

        #endregion
    }
}
