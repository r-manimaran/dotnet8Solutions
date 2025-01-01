using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using PolicyBasedAuthorization;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();

builder.Services.AddOpenApi();
builder.Services.AddTransient<IClaimsTransformation, ClaimsTransformer>();

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

  /*  options.Events = new JwtBearerEvents
    {
        OnTokenValidated = context =>
        {
            var userGroup = context.Principal.FindFirst(AuthConstants.UserGroupClaim)?.Value;
            if (userGroup != AuthConstants.WebClaim && userGroup != AuthConstants.MobileClaim)
            {
                context.Fail("Unauthorized");
            }
            return Task.CompletedTask;
        }
    };*/


});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(AuthConstants.UserGroupWeb, policy =>
    {
        policy.RequireClaim(AuthConstants.UserGroupClaim, AuthConstants.WebClaim);
    });

    options.AddPolicy(AuthConstants.UserGroupMobile, policy =>
    {
        policy.RequireRole("guest");
        policy.RequireClaim(AuthConstants.UserGroupClaim,AuthConstants.MobileClaim);
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.MapGet("/products-minimal", () =>
{
    var products = new[]
            {
                new {Id=1,Name="Laptop"},
                new {Id=2,Name="Mouse"},
                new {Id=3,Name="Mobile"}
            };
    return products;
})
.RequireAuthorization(AuthConstants.UserGroupWeb)
.WithOpenApi();

app.Run();
