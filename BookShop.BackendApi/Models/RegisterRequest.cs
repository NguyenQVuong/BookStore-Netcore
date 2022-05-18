using System;
using System.ComponentModel.DataAnnotations;

namespace BookShop.BackendApi.Models
{
    public class RegisterRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
        [DataType(DataType.Date)]
        public DateTime DOB { get; set; }

    }
}
