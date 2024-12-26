using Microsoft.Extensions.Options;

namespace Validation.Api;

public static class OptionBuilderExtensions
{
    public static OptionsBuilder<TOptions> ValidateFluentValidation<TOptions>(
        this OptionsBuilder<TOptions> builder)
        where TOptions : class
    {
        builder.Services.AddSingleton<IValidateOptions<TOptions>>(
            serviceProvider => 
            new FluentValidationOptions<TOptions>(serviceProvider,
                                                  builder.Name));
        return builder;
    }
}
