using System.Collections.Concurrent;
using System.Threading.Channels;
using ThumbnailGenerator.Models;
using ThumbnailGenerator.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<ImageService>();
builder.Services.AddSingleton(_ =>
{
    var channel = Channel.CreateBounded<ThumbnailGeneratorJob>(new BoundedChannelOptions(100)
    {
        FullMode = BoundedChannelFullMode.Wait
    });
    return channel;
});
builder.Services.AddSingleton<ConcurrentDictionary<string, ThumbnailGenerationStatus>>();
builder.Services.AddHostedService<ThumbnailGenerationService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
