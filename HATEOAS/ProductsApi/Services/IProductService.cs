using ProductsApi.Dtos;

namespace ProductsApi.Services;

public interface IProductService
{
    Task<ProductResponse> GetProductAsync(Guid id);
    Task<PagedList<ProductResponse>> GetProductsAsync(string? searchTerm, string? sortColumn, string? sortOrder, int page, int pageSize);
    Task<ProductResponse> CreateProductAsync(ProductRequest productRequest);
    Task<ProductResponse> UpdateProductAsync(Guid id, ProductRequest productRequest);
    Task DeleteProductAsync(Guid id);
}
