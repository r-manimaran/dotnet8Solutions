﻿using System.ComponentModel.DataAnnotations;

namespace Postgres_jwtAuth.Api.Models;

public class TokenInfo
{
    public int Id { get; set; }
    
    [Required]
    [MaxLength(30)]
    public string UserName { get; set; } = string.Empty;
    [Required]
    [MaxLength(200)]
    public string RefreshToken { get; set; } = string.Empty;
    [Required]
    public DateTime ExpiredAt { get; set; }
}
