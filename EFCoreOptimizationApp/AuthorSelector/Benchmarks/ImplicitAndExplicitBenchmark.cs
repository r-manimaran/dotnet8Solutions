using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthorSelector.Benchmarks;

[MemoryDiagnoser]
public class ImplicitAndExplicitBenchmark
{
    private const int AuthorId = 10;
    [Benchmark]
    public async Task<object> ImplicitInclude()
    {
        throw new NotImplementedException();
    }
    [Benchmark]
    public async Task<object> ExplicitInclude()
    {
        throw new NotImplementedException();
    }
}


