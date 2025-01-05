using Microsoft.Extensions.Options;

namespace CompanyApi.Options;

public class DatabaseOptionSetup(IConfiguration configuration) : IConfigureOptions<DatabaseOptions>
{
    private const string ConfigurationSectionName = "DatabaseOptions";
    public void Configure(DatabaseOptions options)
    {
        var connectionString = configuration.GetConnectionString("Database");

        options.ConnectionString = connectionString!;

       configuration.GetSection(ConfigurationSectionName).Bind(options);
    }
}
