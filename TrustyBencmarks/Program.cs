using BenchmarkDotNet.Running;

namespace TrustyBencmarks;

internal class Program
{
    private static void Main(string[] args)
    {
        BenchmarkRunner.Run<BenchamarkMethods>(/*DefaultConfig.Instance.WithOption(ConfigOptions.DisableOptimizationsValidator, true)*/);
    }
}