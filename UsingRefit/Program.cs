using Refit;
using UsingRefit.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Refit Configuration
var accessToken = "eyJhbGciOiJIUzI1NiJ9.eyJhdWQiOiI0MTUzNTY5MjFlMDVjNWEyYzczMTRhZjY5NGUwNTVmNCIsInN1YiI6IjY1ZGUwMjA5NzE5YWViMDE0OGU3MzE1OCIsInNjb3BlcyI6WyJhcGlfcmVhZCJdLCJ2ZXJzaW9uIjoxfQ.q4yHIX0c9ZDMDs_6XLMA4Czw4W_WpUsxgN1Hm_SP-IY";
var refitSettings = new RefitSettings()
{
    AuthorizationHeaderValueGetter = (rq, ct) => Task.FromResult(accessToken),
};

builder.Services.AddRefitClient<ITmdbApi>(refitSettings)
                .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://api.themoviedb.org/3"));



builder.Services.AddRefitClient<IUserApi>()
        .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://jsonplaceholder.typicode.com/"));


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
