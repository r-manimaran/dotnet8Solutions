using ApiHealthChecks.Models;

namespace ApiHealthChecks.Services
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllProducts();
        Task<Product> GetProductById(int id);
    }
}
