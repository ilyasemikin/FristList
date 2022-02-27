using FluentValidation;
using FristList.Service.PublicApi.Contracts.RequestModels.Account;

namespace FristList.Service.PublicApi.Validators.Authorize;

public class AuthorizeModelValidator : AbstractValidator<AuthorizeModel>
{
    public AuthorizeModelValidator()
    {
        RuleFor(m => m.Login)
            .NotNull()
            .NotEmpty();
        RuleFor(m => m.Password)
            .NotNull()
            .NotEmpty();
    }
}