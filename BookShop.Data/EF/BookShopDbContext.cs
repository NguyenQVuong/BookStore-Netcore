using BookShop.Data.Entities;
using BookShop.Data.Enum;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace BookShop.Data.EF
{
    public class BookShopDbContext:IdentityDbContext<User, Role, Guid>
    {
        public BookShopDbContext(DbContextOptions options) :base(options)
        {
        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer(@"Server=.\SQLEXPRESS;Database=BookShop;Trusted");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityUserClaim<Guid>>().ToTable("AppUserClaims");
            modelBuilder.Entity<IdentityUserRole<Guid>>().ToTable("AppUserRoles").HasKey(x => new { x.UserId, x.RoleId });
            modelBuilder.Entity<IdentityUserLogin<Guid>>().ToTable("AppUserLogins").HasKey(x => x.UserId);

            modelBuilder.Entity<IdentityRoleClaim<Guid>>().ToTable("AppRoleClaims");
            modelBuilder.Entity<IdentityUserToken<Guid>>().ToTable("AppUserTokens").HasKey(x => x.UserId);

            modelBuilder.Entity<Cart>().ToTable("Carts");
            modelBuilder.Entity<Cart>().HasKey(x => x.Id);
            modelBuilder.Entity<Cart>().Property(x => x.Id).UseIdentityColumn();
            modelBuilder.Entity<Cart>().HasOne(x => x.Product).WithMany(x => x.Carts).HasForeignKey(x => x.ProductId);
            modelBuilder.Entity<Cart>().HasOne(x => x.User).WithMany(x => x.Carts).HasForeignKey(x => x.UserId);

            modelBuilder.Entity<Category>().ToTable("Categories");
            modelBuilder.Entity<Category>().HasKey(x => x.Id);
            modelBuilder.Entity<Category>().Property(x => x.Id).UseIdentityColumn();
            modelBuilder.Entity<Category>().Property(x => x.Status).HasDefaultValue(Status.Active);

            modelBuilder.Entity<Cronjobs>().ToTable("Cronjobs");
            modelBuilder.Entity<Cronjobs>().HasKey(x => x.Id);
            modelBuilder.Entity<Cronjobs>().Property(x => x.Id).UseIdentityColumn();
            modelBuilder.Entity<Cronjobs>().Property(x => x.Name).IsRequired().IsUnicode(true).HasMaxLength(200);
            modelBuilder.Entity<Cronjobs>().Property(x => x.Cronjob_Express).IsRequired().IsUnicode(false).HasMaxLength(200);
            modelBuilder.Entity<Cronjobs>().Property(x => x.Status).IsRequired();
            modelBuilder.Entity<Cronjobs>().Property(x => x.Function).IsRequired();
            modelBuilder.Entity<Cronjobs>().Property(x => x.LastedRun).IsRequired();

            modelBuilder.Entity<Order>().ToTable("Orders");
            modelBuilder.Entity<Order>().HasKey(x => x.Id);
            modelBuilder.Entity<Order>().Property(x => x.Id).UseIdentityColumn();
            modelBuilder.Entity<Order>().Property(x => x.OrderDate);
            modelBuilder.Entity<Order>().Property(x => x.ShipEmail).IsRequired().IsUnicode(false).HasMaxLength(50);
            modelBuilder.Entity<Order>().Property(x => x.ShipAddress).IsRequired().HasMaxLength(200);
            modelBuilder.Entity<Order>().Property(x => x.ShipName).IsRequired().HasMaxLength(200);
            modelBuilder.Entity<Order>().Property(x => x.ShipPhoneNumber).IsRequired().HasMaxLength(200);
            modelBuilder.Entity<Order>().HasOne(x => x.User).WithMany(x => x.Orders).HasForeignKey(x => x.UserId);

            modelBuilder.Entity<OrderDetail>().ToTable("OrderDetails");
            modelBuilder.Entity<OrderDetail>().HasKey(x => new { x.OrderId, x.ProductId });
            modelBuilder.Entity<OrderDetail>().HasOne(x => x.Order).WithMany(x => x.OrderDetails).HasForeignKey(x => x.OrderId);
            modelBuilder.Entity<OrderDetail>().HasOne(x => x.Product).WithMany(x => x.OrderDetails).HasForeignKey(x => x.ProductId);

            modelBuilder.Entity<Product>().ToTable("Products");
            modelBuilder.Entity<Product>().HasKey(x => x.Id);
            modelBuilder.Entity<Product>().Property(x => x.Id).UseIdentityColumn();
            modelBuilder.Entity<Product>().Property(x => x.Name).IsRequired().HasMaxLength(200);
            modelBuilder.Entity<Product>().Property(x => x.Price).IsRequired();
            modelBuilder.Entity<Product>().Property(x => x.OriginalPrice).IsRequired();
            modelBuilder.Entity<Product>().Property(x => x.ViewCount).IsRequired().HasDefaultValue(0);

            modelBuilder.Entity<ProductInCategory>().ToTable("ProductInCategories");
            modelBuilder.Entity<ProductInCategory>().HasKey(t => new { t.CategoryId, t.ProductId });
            modelBuilder.Entity<ProductInCategory>().HasOne(t => t.Product).WithMany(pc => pc.ProductInCategories).HasForeignKey(pc => pc.ProductId);
            modelBuilder.Entity<ProductInCategory>().HasOne(t => t.Category).WithMany(pc => pc.ProductInCategories).HasForeignKey(pc => pc.CategoryId);

            modelBuilder.Entity<ProductImage>().ToTable("ProductImages");
            modelBuilder.Entity<ProductImage>().HasKey(x => x.Id);
            modelBuilder.Entity<ProductImage>().Property(x => x.Id).UseIdentityColumn();
            modelBuilder.Entity<ProductImage>().Property(x => x.ImagePath).HasMaxLength(200);
            modelBuilder.Entity<ProductImage>().Property(x => x.Caption).HasMaxLength(200);
            modelBuilder.Entity<ProductImage>().HasOne(x => x.Product).WithMany(x => x.ProductImages).HasForeignKey(x => x.ProductId);

            modelBuilder.Entity<Role>().ToTable("Roles");
            modelBuilder.Entity<Role>().Property(x => x.RoleName).HasMaxLength(200).IsRequired();

            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<User>().Property(x => x.FirstName).IsRequired().HasMaxLength(200);
            modelBuilder.Entity<User>().Property(x => x.LastName).IsRequired().HasMaxLength(200);
            modelBuilder.Entity<User>().Property(x => x.DOB).IsRequired();
        }

        //entities

        public DbSet<Cart> Carts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Cronjobs> Cronjobs { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductInCategory> ProductInCategories { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }

    }
}
