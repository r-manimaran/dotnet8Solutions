using EmailTriggerUsingFlluentEmail.ExtensionMethods;
using EmailTriggerUsingFlluentEmail.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddFluentEmail(builder.Configuration);
builder.Services.AddScoped<IEmailService,EmailService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
