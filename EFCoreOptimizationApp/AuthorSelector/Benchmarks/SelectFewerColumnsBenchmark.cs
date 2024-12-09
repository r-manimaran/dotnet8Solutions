using AuthorSelector.DBContext;
using AuthorSelector.DTOs;
using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthorSelector.Benchmarks;

[MemoryDiagnoser]
public class SelectFewerColumnsBenchmark
{
    private const int AuthorId = 10;
    [Benchmark]
    public async Task<object> SelectAll()
    {
        //using var context = new AppDbContext();
        //var authors = await context.Authors
        //            .Where(x => x.Id == AuthorId)
        //            .Select(a => new AuthorDto
        //            {
        //                AuthorId = a.Id,
        //                AuthorNickName = a.NickName,
        //                AuthorAge = a.Age,
        //            }).ToList();
        //return authors;
        throw new NotImplementedException();
    }
    [Benchmark]
    public async Task<object> SelectNeededColumns()
    {
        throw new NotImplementedException();
    }
}
