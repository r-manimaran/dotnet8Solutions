using FluentValidation;

namespace CoreApi.DTOs.Validators;

public class UserDtoValidator : AbstractValidator<UserDto>
{
    public UserDtoValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(50);
        RuleFor(x=>x.LastName).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Address).SetValidator(new AddressDtoValidator());
        RuleFor(x => x.EmailAddress).NotEmpty();
        RuleForEach(x => x.Tags).MaximumLength(20);
    }
}
