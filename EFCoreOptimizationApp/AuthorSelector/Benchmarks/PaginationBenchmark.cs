using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthorSelector.Benchmarks;

[MemoryDiagnoser]
public class PaginationBenchmark
{
    private const int PageSize = 10;
    private const int Page = 1;

    [Benchmark]
    public async Task<object> PageAndCountTogether()
    {
        throw new NotImplementedException();
    }

    [Benchmark]
    public async Task<object> PageAndCountSeparately()
    {
        throw new NotImplementedException();
    }
}
