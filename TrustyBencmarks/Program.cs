using BenchmarkDotNet.Running;

namespace TrustyBencmarks;

internal class Program
{
    private static void Main(string[] args)
    {
        BenchmarkRunner.Run<BencmarkTrustyMethods>(/*DefaultConfig.Instance.WithOption(ConfigOptions.DisableOptimizationsValidator, true)*/);
    }
}