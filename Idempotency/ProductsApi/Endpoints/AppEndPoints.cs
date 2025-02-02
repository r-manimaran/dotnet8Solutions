using Microsoft.AspNetCore.Mvc;
using ProductsApi.DTOs;
using ProductsApi.Services;

namespace ProductsApi.Endpoints;

public static class AppEndPoints
{
    public static void MapApiEndpoints(this IEndpointRouteBuilder app)
    {
       var group = app.MapGroup("/api/products");
        group.MapPost("/", async(
                                      CreateProductRequest request,
                                      [FromHeader(Name ="X-Idempotency-Key")]string requestId,
                                      IProductService productService,
                                      IIdemptencyService idemptencyService) =>
        {
            if(!Guid.TryParse(requestId, out Guid parsedRequestId))
            {
                return Results.BadRequest();
            }

            try
            {
                // check if the request is already processed
                if(await idemptencyService.RequestExistsAsync(parsedRequestId))
                {

                    return Results.Ok(new {
                        message = "Request already processed",
                        isRepeatedRequest = true});
                }

                // store the idempotent record before processing
                await idemptencyService.CreateRequestAsync(parsedRequestId,"CreateProduct");

                await productService.CreateProduct(request);

                return Results.Ok();

            }
            catch(Exception ex)
            {
                return Results.Problem(ex.Message);
            }
           
        });

        group.MapGet("/", async (string? searchTerm, string? sortColumn, string? sortOrder,
                              int page, int pageSize,
                              IProductService productService) =>
        {
            var products = await productService.GetProducts(searchTerm, sortColumn, sortOrder, page, pageSize);
            return Results.Ok(products);
        });
    }
}
