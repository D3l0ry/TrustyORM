namespace TrustyBencmarks;

internal class Program
{
    private static void Main(string[] args)
    {
        BenchmarkDotNet.Running.BenchmarkRunner
            .Run<BenchamarkMethods>(/*DefaultConfig.Instance.WithOption(ConfigOptions.DisableOptimizationsValidator, true)*/);
    }
}