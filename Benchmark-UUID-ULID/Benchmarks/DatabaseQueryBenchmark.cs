using System;
using BenchmarkDotNet.Attributes;
using Microsoft.EntityFrameworkCore;

namespace Benchmark_UUID_ULID.Benchmarks;

[MemoryDiagnoser]
public class DatabaseQueryBenchmark
{
    [Params(1_000_0)]
    public int Size { get; set; }

    [Benchmark]
    public async Task<bool> Query_Table1_Int()
    {
        using var context = new AppDbContext();

        await context.Table1.AsNoTracking()
                .OrderBy(x => x.Id)
                .Take(Size)
                .ToListAsync();
        return true;
    }

    [Benchmark]
    public async Task<bool> Query_Table2_UUID()
    {
         using var context = new AppDbContext();

        await context.Table1.AsNoTracking()
                .OrderBy(x => x.Id)
                .Skip(100)
                .Take(Size)
                .ToListAsync();
        return true;
    }

    [Benchmark]
    public async Task<bool> Query_Table3_ULID()
    {
         using var context = new AppDbContext();

        await context.Table3.AsNoTracking()
                .OrderBy(x => x.Id)
                .Skip(100)
                .Take(Size)
                .ToListAsync();
        return true;
    }

    [Benchmark]
    public async Task<bool> Query_Table4_ULID_Binary()
    {
         using var context = new AppDbContext();

        await context.Table4.AsNoTracking()
                .OrderBy(x => x.Id)
                .Skip(100)
                .Take(Size)
                .ToListAsync();
        return true;
    }

    [Benchmark]
    public async Task<bool> Query_Table5_DateTimeClustered(){
         using var context = new AppDbContext();

        await context.Table5.AsNoTracking()
                .OrderBy(x => x.CreatedOnUtc)
                .Skip(100)
                .Select(x=> new {x.Id})
                .Take(Size)
                .ToListAsync();
        return true;
    }

}
