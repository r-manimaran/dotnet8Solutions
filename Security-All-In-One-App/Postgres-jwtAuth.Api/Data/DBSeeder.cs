using Microsoft.AspNetCore.Identity;
using Postgres_jwtAuth.Api.Constants;
using Postgres_jwtAuth.Api.Models;

namespace Postgres_jwtAuth.Api.Data;

public class DBSeeder
{
    public static async Task SeedData(IApplicationBuilder app)
    {
        // create a scoped service to resolve dependencies
        using var scope = app.ApplicationServices.CreateScope();

        //resolve the logger service
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<DBSeeder>>();

        try
        {
            // resolve other dependencies
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            // check if the user exists to prevent duplicate seeding
            if (userManager.Users.Any() == false)
            {
                var user = new ApplicationUser
                {
                    Name = "Admin",
                    UserName = "admin@test.com",
                    Email = "admin@test.com",
                    EmailConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString()
                };

                // Create Admin role if it doesn't exists
                if((await roleManager.RoleExistsAsync(Roles.Admin)) == false)
                {
                    logger.LogInformation("Admin role is creating.");
                    var roleResult = await roleManager.CreateAsync(new IdentityRole(Roles.Admin));

                    if(roleResult.Succeeded == false)
                    {
                        var roleErros = roleResult.Errors.Select(e=>e.Description);
                        logger.LogError($"Failed to create Admin role. Errors {string.Join(", ", roleErros)}");
                    }

                    logger.LogInformation("Admin role is created.");
                }

                // Attempt to create admin user
                var createUserResult = await userManager.CreateAsync(user, password:"Admin@123");

                // Validate user creation
                if(createUserResult.Succeeded == false)
                {
                    var errors = createUserResult.Errors.Select(e=>e.Description);
                    logger.LogError($"Failed to create Admin user. Errors {string.Join(",", errors)}");
                    return;
                }

                // adding role to user
                var addUserToRoleResult = await userManager
                        .AddToRoleAsync(user: user, role: Roles.Admin);
                if(addUserToRoleResult.Succeeded == false)
                {
                    var errors = addUserToRoleResult.Errors.Select(e => e.Description);
                    logger.LogError($"Failed to add admin role to user. Errors:{string.Join(",",errors)}");
                }
                logger.LogInformation("Admin user is created");

            }

        }
        catch (Exception ex)
        {
            logger.LogCritical(ex.Message);
        }
    }
}
