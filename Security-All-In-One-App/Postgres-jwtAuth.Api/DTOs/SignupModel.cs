using System.ComponentModel.DataAnnotations;

namespace Postgres_jwtAuth.Api.DTOs;

public class SignupModel
{
    [Required]
    [MaxLength(30)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(30)]
    [EmailAddress]
    public string EmailAddress { get; set; }

    [Required]
    [MaxLength(30)]
    public string Password { get; set; } = string.Empty;
}
