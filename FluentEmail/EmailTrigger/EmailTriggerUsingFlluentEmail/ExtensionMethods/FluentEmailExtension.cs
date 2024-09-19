namespace EmailTriggerUsingFlluentEmail.ExtensionMethods;

public static class FluentEmailExtension
{
    public static void AddFluentEmail(this IServiceCollection services, IConfiguration configuration)
    {
      var emailSettings = configuration.GetSection("EmailSettings");
      var defaultFromEmail = emailSettings["DefaultFromEmail"];
      var host = emailSettings["SMTPSetting:Host"];
      var port = emailSettings.GetValue<int>("SMTPSetting:Port");

        services.AddFluentEmail(defaultFromEmail)
            .AddSmtpSender(host, port)
            .AddRazorRenderer();
    }
}
