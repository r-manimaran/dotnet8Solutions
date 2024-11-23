using System;
using Microsoft.EntityFrameworkCore;
using Pagination.Domain;
using Pagination.Domain.Models;
using Pagination.Infrastructure.Context;

namespace Pagination.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _dbContext;

    public ProductRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

     public async Task<PagedResponseOffset<Product>> GetWithOffsetPagination(int pageNumber, int pageSize)
    {
        var totalRecords = await _dbContext.Products.AsNoTracking().CountAsync();

        var products = await _dbContext.Products.AsNoTracking()
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        var pagedResponse = new PagedResponseOffset<Product>(products, pageNumber, pageSize,totalRecords);
        return pagedResponse;
    }

    public async Task<PagedResponseKeyset<Product>> GetWithKeysetPagination(int reference, int pageSize)
    {
        // Input validation
        if (pageSize <= 0)
            throw new ArgumentException("Page size must be greater than zero.", nameof(pageSize));

        // 
       var products = await _dbContext.Products.AsNoTracking()            
                        .OrderBy(p => p.Id)
                        .Where(p => p.Id > reference)
                        .Take(pageSize)
                        .Select(p => new Product
                         { Id = p.Id, 
                           Name = p.Name,
                           Price = p.Price,
                           CategoryId = p.CategoryId,
                           CreatedAt =p.CreatedAt,
                           Stock = p.Stock
                         })
                        .ToListAsync();

        //var newReference = products.Count != 0 ? products.Last().Id : reference;
        var newReference =  products.Any() ? products[^1].Id : reference;

        var pagedResponse = new PagedResponseKeyset<Product>(products, newReference);
        return pagedResponse;
    }

   
}
