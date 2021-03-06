using FluentValidation;
using FristList.Service.PublicApi.Contracts.RequestModels.Activities;

namespace FristList.Service.PublicApi.Validators.Activities;

public class AddActivityModelValidator : AbstractValidator<AddActivityModel>
{
    public AddActivityModelValidator()
    {
        RuleFor(m => m.BeginAt)
            .LessThan(m => m.EndAt);
    }
}