using Microsoft.AspNetCore.Mvc;
using ProductsApi.Dtos;
using ProductsApi.Services;

namespace ProductsApi.Endpoints;

public static class ApiEndpoints
{
    public static void MapProductsEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/");

        group.MapGet("products/{id:guid}", async (Guid id, [FromServices] IProductService productService) =>
        {
            try
            {
                var prodResponse = await productService.GetProductAsync(id);
                
                return Results.Ok(prodResponse);
            }
            catch (Exception ex)
            {
                //logger.LogError(ex, "Error getting product by id");
            }
            return Results.NotFound();
        }).WithName("GetProduct");

        group.MapGet("products", async (string? searchTerm, string? sortColumn, string? sortOrder,
                                               int page, int pageSize, [FromServices] IProductService productService) =>
        {
            var prodResponse = await productService.GetProductsAsync(searchTerm, sortColumn, sortOrder, page, pageSize);

            return Results.Ok(prodResponse);
        }).WithName("GetProducts");

        group.MapPost("products", async ([FromBody] ProductRequest productRequest,
                                               [FromServices] IProductService productService) =>
        {
            var prodResponse = await productService.CreateProductAsync(productRequest);

            return Results.Created($"/api/products/{prodResponse.Id}", prodResponse);
        }).WithName("CreateProduct");

        group.MapPut("products/{id:guid}", async (Guid id, [FromBody] ProductRequest productRequest, 
            [FromServices]IProductService productService) =>
        {
            var prodResponse = await productService.UpdateProductAsync(id, productRequest);

            return Results.Ok(prodResponse);
        }).WithName("UpdateProduct");

        group.MapDelete("products/{id:guid}", async (Guid id, [FromServices] IProductService productService) =>
        {
            await productService.DeleteProductAsync(id);
            return Results.NoContent();
        }).WithName("DeleteProduct");
    }
}
