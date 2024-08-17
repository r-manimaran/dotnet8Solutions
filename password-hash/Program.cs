using Microsoft.EntityFrameworkCore;
using password_hash;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(o=> o.CustomSchemaIds(id => id.FullName!.Replace('+','-')));

builder.Services.AddDbContext<AppDbContext>(o => o.UseInMemoryDatabase("db"));

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddSingleton<IPasswordHasher, PasswordHasher>();

builder.Services.AddScoped<RegisterUser>();
builder.Services.AddScoped<LoginUser>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/", () => "Hello World!");

UserEndpoints.Map(app);

app.Run();

