namespace Axh.TextOptimization.Contracts.Config
{
    public interface ITextOptimizationConfig
    {
        string CsvFile { get; }

        bool UpdateCachedStringPatterns { get; }

        bool RunGeneticAlgorithm { get; }
    }
}