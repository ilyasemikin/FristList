using FluentValidation;
using FristList.Service.PublicApi.Contracts.RequestModels.Account;

namespace FristList.Service.PublicApi.Validators.Account;

public class RegisterModelValidator : AbstractValidator<RegisterModel>
{
    public RegisterModelValidator()
    {
        RuleFor(m => m.UserName)
            .NotNull();
        RuleFor(m => m.Password)
            .NotNull()
            .Equal(m => m.ConfirmPassword);
        RuleFor(m => m.ConfirmPassword)
            .NotNull();
    }
}