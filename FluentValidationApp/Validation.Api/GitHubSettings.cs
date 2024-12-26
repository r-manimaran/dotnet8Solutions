using System.ComponentModel.DataAnnotations;

namespace Validation.Api;

public sealed class GitHubSettings
{
    public const string ConfigurationSection = "GitHub";

    [Required, Url]
    public string BaseAddress { get; set; } = string.Empty;
    [Required]
    public string AccessToken { get; set; } = string.Empty;
    [Required]
    public string UserAgent { get; set; } = string.Empty;   
}
