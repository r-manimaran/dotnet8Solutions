namespace EF.Pagination.Benchmark;
using BenchmarkDotNet.Attributes;
using Microsoft.Diagnostics.Tracing.Parsers.Kernel;
using Microsoft.EntityFrameworkCore;

[MemoryDiagnoser]
public class PaginationBenchmark
{

    private const int Page = 9900;
    private const int PageSize = 10;
    private const int cursor = 0;

    [Benchmark]
    public async Task<object> NaviePagination()
    {
        using var context = new AppDbContext();

        // Load all of the products from DB
        var result = await context.Products
                    .Select(s => new Product
                    {
                        Id = s.Id,
                        Name = s.Name,
                        Price = s.Price
                    }).ToListAsync();

        // Doing pagination in memory
        var pagedProducts = result
                        .Skip((Page - 1) * PageSize)
                        .Take(PageSize)
                        .ToList();
        var totalCount = result.Count;
        return (pagedProducts, totalCount);

    }

    [Benchmark]
    public async Task<object> UsingOffSetPagination()
    {
        using var context = new AppDbContext();

        // Doing pagination in DB. Form the query
        var query =  context.Products
                    .Select(s => new Product
                    {
                        Id = s.Id,
                        Name = s.Name,
                        Price = s.Price
                    });

        var result = await query
                    .Skip((Page - 1) * PageSize)
                    .Take(PageSize)
                    .ToListAsync();                   

        var totalCount = await query.CountAsync();
        return (result, totalCount);
    }

     [Benchmark]
     public async Task<object> UsingCursorPagination()
     {
         using var context = new AppDbContext();

         // Doing pagination in DB. Form the query
         var query =  context.Products
                     .Select(s => new Product
                     {
                         Id = s.Id,
                         Name = s.Name,
                         Price = s.Price
                     });
         var result =  await query
                        .Where(s=>s.Id > cursor)
                        .Take(PageSize)
                        .ToListAsync();
        var nextCursor = result[^1].Id;
        //var nextCursor = result.LastOrDefault()?.Id ?? 0;
         return (result, nextCursor);
     }
}
