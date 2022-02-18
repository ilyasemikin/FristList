using FluentValidation;
using FristList.Service.PublicApi.Models.Account;

namespace FristList.Service.PublicApi.Validators.Account;

public class LoginModelValidator : AbstractValidator<LoginModel>
{
    public LoginModelValidator()
    {
        RuleFor(m => m.Login)
            .NotNull()
            .NotEmpty();
        RuleFor(m => m.Password)
            .NotNull()
            .NotEmpty();
    }
}