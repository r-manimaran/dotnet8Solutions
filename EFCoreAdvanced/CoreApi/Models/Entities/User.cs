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
    public List<Order> Orders { get; set; } 
}

public class Email
{
    [Key]
    public string EmailAddress { get; set; }
}