using Example.Model;
using Example.Model.Table.Sys;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http.Headers;

namespace Example.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var user = new User { Name="zhangsan", Birthday= DateTime.Parse("2019-12-03") };

            if (user is BaseEntity)
            {
                Test1(user as BaseEntity);

            }

            IQueryable<User> query = null;

            Func<IQueryable<User>, IOrderedQueryable<User>> orderBy = p => p.OrderBy(w => w.Sort);

            //orderBy(query);

            


            Console.ReadLine();
        }

        public static void Test1(BaseEntity entity)
        {
            entity.Id = Guid.NewGuid().ToString();
            entity.CreateTime = DateTime.Now;
        }

        public static void Test2<TEntity,Key>(Expression<Func<TEntity, bool>> predicate, int pageIndex, int pageSize, Expression<Func<TEntity, Key>> orderBy, bool orderByIsDesc)
        { 
        
        }
    }


}
