using FluentValidation;

namespace Validation.Api;

public sealed class GitHubSettingsValidator:AbstractValidator<GitHubSettings>
{
    public GitHubSettingsValidator()
    {
        RuleFor(x => x.BaseAddress)
            .NotEmpty().WithMessage("Base Address should not empty");

        RuleFor(x=>x.BaseAddress)
            .Must(baseAddress=> Uri.TryCreate(baseAddress, UriKind.Absolute, out _))
            .When(x=>!string.IsNullOrWhiteSpace(x.BaseAddress))
            .WithMessage($"{nameof(GitHubSettings.BaseAddress)} must be a valid URL");

        RuleFor(x => x.AccessToken).NotEmpty().WithMessage("AccessToken should not empty");
        RuleFor(x => x.UserAgent).NotEmpty().WithMessage("UserAgent should not empty");
    }
}
