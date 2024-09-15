//Call Benchmark and run the CartesionExplosionBechmark methods
using BenchmarkDotNet.Running;
using CartesionExplosion.Benchmark;

public class Program
{
    public static void Main(string[] args)
    {
        BenchmarkRunner.Run<CartesionExplosionBenchmark>();
        Console.ReadKey();
    }
}