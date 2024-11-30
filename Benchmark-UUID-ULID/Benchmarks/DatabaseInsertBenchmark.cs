using System;
using System.Data.Common;
using Benchmark_UUID_ULID.Entities;
using BenchmarkDotNet.Attributes;

namespace Benchmark_UUID_ULID.Benchmarks;

[MemoryDiagnoser]
public class DatabaseInsertBenchmark
{
    [Params(1_000_0)]
    public int Size { get; set; }

    [Benchmark]
    public async Task<bool> Insert_Table1_Int()
    {
        using var context = new AppDbContext();
        for (int i=0; i<Size; i++)
        {
            context.Table1.Add(new Table1_Int());
        }
        await context.SaveChangesAsync();
        return true;
    }

    [Benchmark]
    public async Task<bool> Insert_Table2_UUID()
    {
         using var context = new AppDbContext();
        for (int i=0; i<Size; i++)
        {
            context.Table2.Add(new Table2_Guid
            {
                Id = Guid.NewGuid()
            });
        }
        await context.SaveChangesAsync();
        
        return true;
    }

    [Benchmark]
    public async Task<bool> Insert_Table3_ULID()
    {
        using var context = new AppDbContext();
        for (int i=0; i<Size; i++)
        {
            context.Table3.Add(new Table3_Ulid
            {
                Id = Ulid.NewUlid()
            });
        }
        await context.SaveChangesAsync();
        
        return true;
    }

    [Benchmark]
    public async Task<bool> Insert_Table4_ULID_Binary()
    {
        using var context = new AppDbContext();

        for(int i=0; i <Size; i++)
        {
            context.Table4.Add(new Table4_UlidBinary 
            {
                Id = Ulid.NewUlid()
            });
        }

        await context.SaveChangesAsync();

        return true;
    }

    [Benchmark]
    public async Task<bool> Insert_Table5_DateTimeClustered()
    {
        using var context = new AppDbContext();

        for(int i=0; i <Size; i++)
        {
            context.Table5.Add(new Table5_DateTime 
            {
                Id = Guid.NewGuid()
            });
        }

        await context.SaveChangesAsync();
        
        return true;
    }


}
