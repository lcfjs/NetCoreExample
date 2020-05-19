using Example.Model.Table.Sys;
using Microsoft.EntityFrameworkCore;
using System;

namespace Example.Repository
{
    public class ExampleDbContext : DbContext
    {
        public ExampleDbContext(DbContextOptions<ExampleDbContext> options)
           : base(options)
        {

        }

        #region DbSet

        public DbSet<User> Sys_User { get; set; }
        #endregion
    }
}
