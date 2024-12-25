using Microsoft.AspNetCore.Http.HttpResults;
using Asp.Versioning;
using Asp.Versioning.Builder;
namespace MinimalApi.Versioning.Endpoints;

public static class Endpoints
{
    public static void MapProductsEndpoints(this IEndpointRouteBuilder app)
    {
        ApiVersionSet apiVersionSet = app.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1))
            .HasApiVersion(new ApiVersion(2))
            .ReportApiVersions()
            .HasDeprecatedApiVersion(new ApiVersion(1))
            .Build();

        RouteGroupBuilder routeGroupBuilder = app.MapGroup("api/v{apiVersion:apiVersion}")
                                                  .WithApiVersionSet(apiVersionSet)
                                                  .WithTags("Products");

        routeGroupBuilder.MapPost("products", () =>
        {
            return Results.Ok("v1: Product created successfully");
        }).MapToApiVersion(1);

        routeGroupBuilder.MapGet("products/{id}", (int id) =>
        {
            return Results.Ok($"v1: Here is the Product with Id:{id}");
        }).MapToApiVersion(1);


        //Version 2
        routeGroupBuilder.MapPost("products", () =>
        {
            return Results.Ok("v2: Product created successfully");
        }).MapToApiVersion(2);

        routeGroupBuilder.MapGet("products/{id}", (int id) =>
        {
            return Results.Ok($"v2: Here is the Product with Id:{id}");
        }).MapToApiVersion(2);
    }
}
