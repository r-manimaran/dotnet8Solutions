using Carter;
using CoreApi.Data;
using CoreApi.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace CoreApi.Endpoints;

public class ApiEndpoints : CarterModule
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/users").WithOpenApi().WithTags("Users");

        group.MapGet("/{id}", GetUserById)
             .WithName("GetUserById");

        group.MapGet("/", SearchUsers)
             .WithMetadata("Search Users")
             .WithName("SearchUsers");

    }

    private async Task<IActionResult> GetUserById(int id, IUserRepository userRepository)
    {
        var user = await userRepository.GetUserWithRawSql(id);
        return new OkObjectResult(user);
    }

    private async Task<IActionResult> SearchUsers(string? searchTerm, string? exactMatch, IUserRepository userRepository)
    {
        var users = await userRepository.SearchUsers(searchTerm, exactMatch);

        return new OkObjectResult(users);
    }
}
