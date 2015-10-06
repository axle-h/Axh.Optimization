namespace Axh.TextOptimization.Config
{
    using System.Configuration;

    using Axh.TextOptimization.Contracts.Config;

    public class TextOptimizationConfig : ITextOptimizationConfig
    {
        private const string CsvFileKey = "TestOptimization_CsvFile";
        private const string UpdateCachedStringPatternsKey = "TestOptimization_UpdateCachedStringPatterns";
        private const string RunGeneticAlgorithmKey = "TestOptimization_RunGeneticAlgorithm";

        public TextOptimizationConfig()
        {
            this.CsvFile = ConfigurationManager.AppSettings[CsvFileKey];
            this.UpdateCachedStringPatterns = bool.Parse(ConfigurationManager.AppSettings[UpdateCachedStringPatternsKey]);
            this.RunGeneticAlgorithm = bool.Parse(ConfigurationManager.AppSettings[RunGeneticAlgorithmKey]);
        }

        public string CsvFile { get; }

        public bool UpdateCachedStringPatterns { get; }

        public bool RunGeneticAlgorithm { get; }
    }
}
