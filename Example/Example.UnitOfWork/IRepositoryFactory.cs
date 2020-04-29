using System;
using System.Collections.Generic;
using System.Text;

namespace Example.UnitOfWork
{
    public interface IRepositoryFactory
    {
        /// <summary>
        /// 获取实体仓储
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        IRepository<TEntity> GetRepository<TEntity>() where TEntity : class;

        /// <summary>
        /// 获取自定义仓储
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T GetCustomRepository<T>();
    }
}
