using System.ComponentModel.DataAnnotations;

namespace CoreApi.Models.Entities;

public class User
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public Address Address { get; set; } // Owned Type
    public Email EmailAddress { get; set; }
    public UserMetadata Metadata { get; set; } //Json conversion
    public bool IsDeleted { get; set; }
    public virtual List<Order> Orders { get; set; } 
}

public record Email
{
    public Email(string value)
    {
        if(string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Email address is required");
        }

        if (!IsValidEmail(value))
        {
            throw new ArgumentException("Invalid email address");
        }
        Value = value;
    }

    public string Value { get; set; }

    private static bool IsValidEmail(string email)
    {
        return new EmailAddressAttribute().IsValid(email);
    }
}