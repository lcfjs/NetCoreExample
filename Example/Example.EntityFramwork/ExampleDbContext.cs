using Example.Model;
using Example.Model.Table.Sys;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Example.EntityFramwork
{
    public class ExampleDbContext : DbContext
    {
        public ExampleDbContext(DbContextOptions<ExampleDbContext> options)
           : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var item in modelBuilder.Model.GetEntityTypes().Where(e => typeof(BaseEntity).IsAssignableFrom(e.ClrType)))
            {
                if (modelBuilder.Entity(item.ClrType).Property("IsDelete") == null)
                {
                    continue;
                }
                modelBuilder.Entity(item.ClrType).Property<Boolean>("IsDelete");
                var parameter = Expression.Parameter(item.ClrType, "e");
                var body = Expression.Equal(Expression.Call(typeof(EF), nameof(EF.Property), new[] { typeof(bool) }, parameter, Expression.Constant("IsDelete")), Expression.Constant(false));
                modelBuilder.Entity(item.ClrType).HasQueryFilter(Expression.Lambda(body, parameter));
            }

            base.OnModelCreating(modelBuilder);
        }

        #region DbSet

        public DbSet<User> Sys_User { get; set; }
        #endregion
    }
}
