using EF.Pagination.Benchmark;

// use for only initial seeding
/*
using (var context = new AppDbContext())
{
    Console.WriteLine("Application Started");
    context.Database.EnsureCreated();

    DataSeeder.SeedData(context);
}
*/

BenchmarkDotNet.Running.BenchmarkRunner.Run<PaginationBenchmark>();