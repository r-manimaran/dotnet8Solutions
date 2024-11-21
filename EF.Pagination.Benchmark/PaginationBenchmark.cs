namespace EF.Pagination.Benchmark;
using BenchmarkDotNet.Attributes;

[MemoryDiagnoser]
public class PaginationBenchmark
{
    
    private const int Page =9900;
    private const int PageSize = 10;
    private const int cursor=0;    

}
