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

        group.MapGet("/users/{id}", GetUserById)
             .WithName("GetUserById");
        
    }

    private async Task<IActionResult> GetUserById(int id, IUserRepository userRepository)
    {
        var user = await userRepository.GetUserWithRawSql(id);
        return new OkObjectResult(user);
    }
}
