using CustomAuthorizeAttribute.Authorization.Requirements;
using CustomAuthorizeAttribute.Authorization.Requirements.Handlers;
using CustomAuthorizeAttribute.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<SessionValidationService>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<IAuthorizationHandler, SessionHandler>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, o => { });
builder.Services.AddAuthorization(opt =>
{
    opt.AddPolicy("SessionPolicy", policy =>
    {
        policy.Requirements.Add(new SessionRequirement("X-Session-Id"));
    });
});
builder.Services.AddControllers();

builder.Services.AddOpenApi();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
