using FluentValidation;
using Validation.Api.Models;

namespace Validation.Api.ModelValidators;

public class PersonalInfoValidator : AbstractValidator<PersonalInfo>
{
    public PersonalInfoValidator()
    {
        RuleFor(x=>x.FirstName).NotEmpty().WithMessage($"{nameof(PersonalInfo.FirstName)} is required!");
        RuleFor(x=>x.LastName).NotEmpty().WithMessage($"{nameof(PersonalInfo.LastName)} is required!");       
    }
}
