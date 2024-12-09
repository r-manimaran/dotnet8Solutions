using BenchmarkDotNet.Running;

namespace AuthorSelector;

public class Program 
{
    public static void Main(string[] args) 
    {
      Console.WriteLine("EFCore query optimize.");

      BenchmarkRunner.Run<QueryFetchBenchmarks>();
    }

   
}
