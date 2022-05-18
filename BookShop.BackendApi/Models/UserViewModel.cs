using System;

namespace BookShop.BackendApi.Models
{
    public class UserViewModel
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DOB { get; set; }
        public string Email { get; set; }   
        public string PhoneNumber { get; set; }
        public string Avatar { get; set; }
    }
}
