using AuthorSelector.DBContext;
using BenchmarkDotNet.Attributes;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthorSelector;
[MemoryDiagnoser]
public class NoTrackingBenchmark
{
    private const int BookId = 3840;
    [Benchmark]
    public async Task<object> WithTracking()
    {
        using var context = new AppDbContext();
        var response = context.Books
               .Where(x => x.Id == BookId)
               .ToList();
        return response;

    }

    [Benchmark]
    public async Task<object> WithoutTracking()
    {
        using var context = new AppDbContext();
        var response = context.Books
               .AsNoTracking()
               .Where(x => x.Id == BookId)
               .ToList();
        return response;

    }
}
