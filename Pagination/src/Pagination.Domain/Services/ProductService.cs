using Pagination.Domain.Models;

namespace Pagination.Domain;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }
  
    public async Task<PagedResponseKeyset<Product>> GetWithKeysetPagination(int reference, int pageSize)
    {
        return await _productRepository.GetWithKeysetPagination(reference, pageSize);
    }

    public async Task<PagedResponseOffset<Product>> GetWithOffsetPagination(int pageNumber, int pageSize)
    {
        return await _productRepository.GetWithOffsetPagination(pageNumber, pageSize);
    }
}
