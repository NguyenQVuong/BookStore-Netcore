using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookShop.Data.Entities
{
    public class Role: IdentityRole<Guid>
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }    
    }
}
