using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Database.Context
{
    public class DbContextFactory : IDesignTimeDbContextFactory<FunatDbContext>
    {
        public FunatDbContext CreateDbContext(string[] args)
        {
            DbContextOptionsBuilder<FunatDbContext> optionsBuilder = new DbContextOptionsBuilder<FunatDbContext>();
            _ = optionsBuilder.UseSqlServer(new DatabaseConfiguration().ToString());
            return new FunatDbContext(optionsBuilder.Options);
        }
    }
}
