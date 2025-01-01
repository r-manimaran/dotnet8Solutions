using Microsoft.AspNetCore.Identity;

namespace Postgres_jwtAuth.Api.Models;

public class ApplicationUser : IdentityUser
{
    public string Name { get; set; } = string.Empty;
}

