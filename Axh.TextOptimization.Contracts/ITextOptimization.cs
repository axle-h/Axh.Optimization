namespace Axh.TextOptimization.Contracts
{
    public interface ITextOptimization
    {
        void UpdateCachedStringPatterns();

        void RunGeneticAlgorithm();

        void CheckTop100();

        void RunFromConfig();
    }
}