using ProductsApi.DTOs;
using ProductsApi.Models;

namespace ProductsApi.Services;

public interface IProductService
{
    Task CreateProduct(CreateProductRequest request);
    Task<PaginatedResponse<Product>> GetProducts(string searchTerm, string sortColumn, string sortOrder, int page, int pageSize);
}
