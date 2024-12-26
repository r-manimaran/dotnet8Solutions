using System.Data;
using FluentValidation;
using Validation.Api.Models;

namespace Validation.Api.ModelValidators;

public class UserRegistrationValidator : AbstractValidator<UserRegistrationDto>
{
    public UserRegistrationValidator()
    {
        RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required field")
                .EmailAddress().WithMessage("Invalid Email Format provided");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required field")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters long")
            .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter")
            .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter")
            .Matches("[0-9]").WithMessage("Password must contain at least one digit")
            .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character");
        
        RuleFor(x => x.ConfirmPassword)
            .NotEmpty().WithMessage("Confirm Password is required field")
            .Equal(x => x.Password).WithMessage("Confirm Password must match Password");
        
        RuleFor(x=>x.PhoneNumber)
            .NotEmpty().WithMessage("Phone Number is required field")
            .Matches(@"^\+[1-9]\d{1,14}$").WithMessage("Invalid Phone Number format");
        
        RuleFor(x=>x.PersonalInfo)
            .NotNull().WithMessage("Personal Info is required field")
            .SetValidator(new PersonalInfoValidator());

        RuleFor(x=>x.AddressInfo)
            .NotNull().WithMessage("Address is required field")
            .SetValidator(new AddressInfoValidator());
            
        var minimumAge = 18;
        RuleFor(x=>x.DateOfBirth)
            .NotEmpty().WithMessage("Date of Birth is required field")
            .Must(BeInPast).WithMessage("Date of Birth must be in the past")
            .Must(x=>BeValidAge(x, minimumAge)).WithMessage($"User must be at least {minimumAge} years old");
        
        RuleFor(x=>x.AcceptTerms)
            .Equal(true).WithMessage("Terms must be accepted");
    }
    private static bool BeInPast(DateTime date)
    {
        return date < DateTime.UtcNow;
    }
    private static bool BeValidAge(DateTime date, int minimumAge)
    {
        var age = DateTime.UtcNow.Year - date.Year;
        return age >= minimumAge;
    }
}
