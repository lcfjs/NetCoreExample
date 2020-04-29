using Example.UnitOfWork.Entity;
using Example.WebCore.BaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Example.UnitOfWork
{
    public interface IRepository<TEntity> where TEntity : class
    {
        void Insert(TEntity entity);
        void Insert(TEntity entity, CurrentUserBase userInfo);
        void Insert(IEnumerable<TEntity> entities);
        void Insert(IEnumerable<TEntity> entities, CurrentUserBase userInfo);

        void Update(TEntity entity);
        void Update(TEntity entity, CurrentUserBase userInfo);
        void Update(IEnumerable<TEntity> entities, CurrentUserBase userInfo);

        /// <summary>
        /// 物理删除
        /// </summary>
        /// <param name="entity"></param>
        void Delete(TEntity entity);
        /// <summary>
        /// 物理删除
        /// </summary>
        /// <param name="entities"></param>
        void Delete(IEnumerable<TEntity> entities);
        /// <summary>
        /// 物理删除
        /// </summary>
        /// <param name="predicate"></param>
        void Delete(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 逻辑删除
        /// </summary>
        /// <param name="entity"></param>
        void LogicDelete(TEntity entity, CurrentUserBase userInfo);
        /// <summary>
        /// 逻辑删除
        /// </summary>
        /// <param name="entities"></param>
        void LogicDelete(IEnumerable<TEntity> entities, CurrentUserBase userInfo);
        /// <summary>
        /// 逻辑删除
        /// </summary>
        /// <param name="predicate"></param>
        void LogicDelete(Expression<Func<TEntity, bool>> predicate, CurrentUserBase userInfo);

        TEntity GetFirstOrDefault(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null);
        Task<TEntity> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null);

        TResult GetFirstOrDefault<TResult>(Expression<Func<TEntity, TResult>> selector, Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null);
        Task<TResult> GetFirstOrDefaultAsync<TResult>(Expression<Func<TEntity, TResult>> selector, Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null);

        IEnumerable<TEntity> GetList(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null);
        Task<IEnumerable<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null);

        IEnumerable<TResult> GetList<TResult>(Expression<Func<TEntity, TResult>> selector, Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null);
        Task<IEnumerable<TResult>> GetListAsync<TResult>(Expression<Func<TEntity, TResult>> selector, Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null);

        //IPageList<TEntity> GetPageList( Expression<Func<TEntity, bool>> predicate, int pageIndex, int pageSize, Expression<Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>> orderBy, bool orderByIsDesc); //orderBy(query)
        Task<IPageList<TEntity>> GetPageListAsync(Expression<Func<TEntity, bool>> predicate, int pageIndex, int pageSize, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null);
        Task<IPageList<TResult>> GetPageListAsync<TResult>(Expression<Func<TEntity, bool>> predicate, int pageIndex, int pageSize, Func<IEnumerable<TEntity>, IEnumerable<TResult>> fun, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null);

        IQueryable<TEntity> FromSql(string sql, params object[] parameters);
    }
}
