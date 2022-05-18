using System;
using System.ComponentModel.DataAnnotations;

namespace BookShop.BackendApi.Models
{
    public class UpdateUserRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        [DataType(DataType.Date)]
        public DateTime DOB { get; set; }
    }
}
