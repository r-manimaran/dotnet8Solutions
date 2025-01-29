using System.ComponentModel.DataAnnotations;

namespace CoreApi.DTOs;

public record UserDto(
    string FirstName, string LastName, AddressDto Address, EmailDto EmailAddress, List<string> Tags);

public record AddressDto([Required]string Street, 
                         [Required]string City, 
                         [Required]string State,
    [RegularExpression(@"^\d{5}$")] string ZipCode);

public record EmailDto(
                       [Required]
                       [EmailAddress]
                       [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Invalid email format")]
                       string Email);

