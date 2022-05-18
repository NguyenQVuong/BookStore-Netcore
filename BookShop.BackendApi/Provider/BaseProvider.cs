using BookShop.Data.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;

namespace BookShop.BackendApi.Provider
{
    public class BaseProvider
    {
        public BookShopDbContext db = null;
        public BaseProvider()
        {
            DbContextOptionsBuilder<BookShopDbContext> optionsBuilder = new DbContextOptionsBuilder<BookShopDbContext>();
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("BookShopDb"));
            db = new BookShopDbContext(optionsBuilder.Options);
        }
    }
}
