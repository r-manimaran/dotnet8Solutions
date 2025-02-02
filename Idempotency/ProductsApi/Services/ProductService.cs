using Microsoft.EntityFrameworkCore;
using ProductsApi.Data;
using ProductsApi.DTOs;
using ProductsApi.Models;

namespace ProductsApi.Services;

public class ProductService : IProductService
{
    private readonly AppDbContext _dbContext;
    private readonly ILogger<ProductService> _logger;

    public ProductService(AppDbContext dbContext, ILogger<ProductService> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }
    public async Task CreateProduct(CreateProductRequest request)
    {
        var newProduct = new Product
        {
            Id = new Guid(),
            Name = request.Name,
            Amount = request.Amount,
            Sku = request.Sku,
            Currency = request.Currency,
        };
        _dbContext.Add(newProduct);
       await  _dbContext.SaveChangesAsync();
    }

    public async Task<PaginatedResponse<Product>> GetProducts(string searchTerm, string sortColumn, string sortOrder, int page, int pageSize)
    {
        IQueryable<Product> query = _dbContext.Products;

        if (!string.IsNullOrEmpty(searchTerm))
        {
            query = query.Where(p => p.Name.Contains(searchTerm) || 
                                p.Sku.Contains(searchTerm) ||
                                p.Currency.Contains(searchTerm));
        }
        
        query = sortColumn?.ToLower() switch
        {
            "name" => sortOrder?.ToLower() == "desc" ? query.OrderByDescending(p => p.Name) : query.OrderBy(p => p.Name),
            "amount" => sortOrder?.ToLower() == "desc" ? query.OrderByDescending(p => p.Amount) : query.OrderBy(p => p.Amount),
            "sku" => sortOrder?.ToLower() == "desc" ? query.OrderByDescending(p => p.Sku) : query.OrderBy(p => p.Sku),
            "currency" => sortOrder?.ToLower() == "desc" ? query.OrderByDescending(p => p.Currency) : query.OrderBy(p => p.Currency),
            _ => query.OrderBy(p => p.Name)
        };

        var products = await query
                            .Skip((page-1)*pageSize)
                            .Take(pageSize)
                            .ToListAsync();
                            
        var response = new PaginatedResponse<Product>
        {
            Items = products,
            CurrentPage = page,
            TotalPages = (int)Math.Ceiling(await query.CountAsync() / (double)pageSize),
            TotalCount = await query.CountAsync()          
            
        };
        _logger.LogInformation($"Products retrieved: {response.TotalCount}");
        return response;
    }
}
