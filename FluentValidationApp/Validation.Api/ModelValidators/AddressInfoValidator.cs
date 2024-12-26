using FluentValidation;
using Validation.Api.Models;

namespace Validation.Api.ModelValidators;

public class AddressInfoValidator : AbstractValidator<AddressInfo>
{
    public AddressInfoValidator()
    {
        RuleFor(x => x.Street)
            .NotEmpty()
            .WithMessage($"{nameof(AddressInfo.Street)} is required");
        RuleFor(x => x.City)
            .NotEmpty()
            .WithMessage($"{nameof(AddressInfo.City)} is required");
        RuleFor(x => x.State)
            .NotEmpty()
            .WithMessage($"{nameof(AddressInfo.State)} is required");
        RuleFor(x => x.Country)
            .NotEmpty()
            .WithMessage($"{nameof(AddressInfo.Country)} is required");
        RuleFor(x => x.PostalCode)
            .NotEmpty()
            .WithMessage($"{nameof(AddressInfo.PostalCode)} is required");
    }
}
