using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using RoleBasedAuthorization;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddAuthorization();

builder.Services.AddAuthentication().AddJwtBearer(options =>
{
    var signingKey = builder.Configuration["Authentication:Schemes:Bearer:SigningKeys:0:Value"];
    var validAudiences = builder.Configuration.GetSection("Authentication:Schemes:Bearer:ValidAudiences")
                               .Get<string[]>() ?? new string[] { "http://localhost:5019", "https://localhost:7252" };
   
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        // These values must match what dotnet user-jwts creates
        ValidIssuer = "dotnet-user-jwts",
        ValidAudiences = validAudiences,
        IssuerSigningKey = new SymmetricSecurityKey(
            Convert.FromBase64String(signingKey ?? 
                throw new InvalidOperationException("JWT signing key is not configured.")))
    };

    //Events
    options.Events = new JwtBearerEvents()
    {
        OnTokenValidated = async context =>
        {
            var userRepository = context.HttpContext.RequestServices.GetService<IUserRepository>();
            
            var userRoles = await userRepository.GetUserRolesAsync(context.Principal!.Identity!.Name, context.HttpContext.RequestAborted);
            
            var userClaims = userRoles.Select(role => new Claim(ClaimTypes.Role, role));

            ((ClaimsIdentity)context.Principal!.Identity!).AddClaims(userClaims);
        }

    };
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    // For Scalar
    app.MapScalarApiReference();

    //For Swagger
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapGet("/users-minimal", [Authorize(Roles="Admin")] () =>
{
    var users = new[]
            {
                new { id = 1, Name="James" },
                new { id = 2, Name="Alex" },
                new { id = 3, Name="Philip" }
            };
    return users;
})
 //.RequireAuthorization(new AuthorizeAttribute { Roles = "Admin"}) //Alternate approach to add Authorize attribute
 .WithOpenApi();

app.Run();
