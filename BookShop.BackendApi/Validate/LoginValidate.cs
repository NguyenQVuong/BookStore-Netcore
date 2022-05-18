using BookShop.BackendApi.Models;
using FluentValidation;

namespace BookShop.BackendApi.Validate
{
    public class LoginValidate: AbstractValidator<LoginRequest>
    {
        public LoginValidate()
        {
            RuleFor(x => x.UserName).NotEmpty().WithMessage("Email is required");
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required").MinimumLength(6).WithMessage("Password is at least 6 characters");
        }
    }
}
