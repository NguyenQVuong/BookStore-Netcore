using BookShop.Data.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace eShopSolution.Data.EF
{
    public class BookShopDbContextFactory : IDesignTimeDbContextFactory<BookShopDbContext>
    {
        public BookShopDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

            var connectionString = configuration.GetConnectionString("BookShopDb");

            var optionsBuilder = new DbContextOptionsBuilder<BookShopDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new BookShopDbContext(optionsBuilder.Options);
        }
    }
}
