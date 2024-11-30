
using Benchmark_UUID_ULID;
using Benchmark_UUID_ULID.Benchmarks;

using (var context = new AppDbContext())
{
    Console.WriteLine("Starting...");
    context.Database.EnsureCreated();
}

//BenchmarkDotNet.Running.BenchmarkRunner.Run<DatabaseInsertBenchmark>();
BenchmarkDotNet.Running.BenchmarkRunner.Run<DatabaseQueryBenchmark>();