using FluentValidation;

namespace CoreApi.DTOs.Validators;

public class AddressDtoValidator: AbstractValidator<AddressDto>
{
    public AddressDtoValidator()
    {
        RuleFor(x => x.City).NotEmpty();
        RuleFor(x=>x.Street).NotEmpty();
        RuleFor(x=>x.State).NotEmpty(); 
        RuleFor(x=>x.ZipCode).NotEmpty();
    }
}
