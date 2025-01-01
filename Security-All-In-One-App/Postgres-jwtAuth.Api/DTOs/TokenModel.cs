using System.ComponentModel.DataAnnotations;

namespace Postgres_jwtAuth.Api.DTOs;

public class TokenModel
{
    [Required]
    public string AccessToken { get; set; } = string.Empty;
    [Required]
    public string RefreshToken { get; set; } = string.Empty;
}
