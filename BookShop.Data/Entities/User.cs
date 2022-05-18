using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookShop.Data.Entities
{
    public class User: IdentityUser<Guid>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DOB { get; set; }
        public string Avatar { get; set; }
        public bool Unsubscribe { get; set; }
        public List<Cart> Carts { get; set; }
        public List<Order> Orders { get; set; }
    }
}

