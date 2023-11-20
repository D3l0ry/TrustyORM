using BenchmarkDotNet.Running;

namespace TrustyBencmarks;

internal class Program
{
    private static void Main(string[] args)
    {
        BenchmarkRunner.Run<BenchmarkTrustyMethods>(/*DefaultConfig.Instance.WithOption(ConfigOptions.DisableOptimizationsValidator, true)*/);
        Console.ReadLine();
    }
}