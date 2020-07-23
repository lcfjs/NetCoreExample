using Example.Model;
using Example.Model.Table.Sys;
using System;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http.Headers;
using Example.Core.Extension;
using System.Collections.Generic;
using Example.Core;

namespace Example.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var user = new User { Name = "zhangsan", Birthday = DateTime.Parse("2019-12-03") };

            if (user is BaseEntity)
            {
                Test1(user as BaseEntity);

            }

            IQueryable<User> query = null;

            Func<IQueryable<User>, IOrderedQueryable<User>> orderBy = p => p.OrderBy(w => w.Sort);

            //orderBy(query);

            Test2();


            Console.ReadLine();
        }

        public static void Test1(BaseEntity entity)
        {
            entity.Id = Guid.NewGuid().ToString();
            entity.CreateTime = DateTime.Now;
        }

        public static void Test2<TEntity, Key>(Expression<Func<TEntity, bool>> predicate, int pageIndex, int pageSize, Expression<Func<TEntity, Key>> orderBy, bool orderByIsDesc)
        {

        }

        public static void Test2()
        {
            DataTable table = new DataTable();
            table.Columns.Add("Id");
            table.Columns.Add("Name");
            table.Columns.Add("Sort", typeof(long));
            table.Columns.Add("UpdateTime", typeof(DateTime));

            var row1 = table.NewRow();
            row1["Id"] = Guid.NewGuid().ToString();
            row1["Name"] = "zhangsan";
            row1["Sort"] = DateTime.Now.ToTimestampMillisecond();
            row1["UpdateTime"] = DateTime.Now;
            var row2 = table.NewRow();
            row2["Id"] = Guid.NewGuid().ToString();
            row2["Name"] = "王五";
            row2["Sort"] = DateTime.Now.ToTimestampMillisecond();
            row2["UpdateTime"] = DBNull.Value;
            table.Rows.Add(row1);
            table.Rows.Add(row2);

            var list = table.ToEntity<User>();

            var listUser = new List<User>() {
             new User{ Id=Guid.NewGuid().ToString(),Name="张安", Sort=111111,UpdateTime= DateTime.Now},
             new User{Id=Guid.NewGuid().ToString(),Name="wangwu", Sort=222 },
            };
            var val2 = listUser.ToDataTable();

            var snowflakeId = new SnowflakeId(2, 1);
            for (int i = 0; i < 1000; i++)
            {
                Console.WriteLine(snowflakeId.NextId());
            }
        }
    }


}
