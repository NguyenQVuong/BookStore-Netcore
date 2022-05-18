namespace BookShop.BackendApi.Models
{
    public class LoginRequest
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
