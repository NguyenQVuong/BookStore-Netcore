using System;
using System.Collections.Generic;
using System.Text;

namespace BookShop.Data.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public decimal OriginalPrice { get; set; }
        public DateTime DateCreated { get; set; }
        public int ViewCount { get; set; }
        public bool? IsFeatures { get; set; }
        public List<Cart> Carts { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }
        public List<ProductImage> ProductImages { get; set; }
        public List<ProductInCategory> ProductInCategories { get; set; }
    }
}
