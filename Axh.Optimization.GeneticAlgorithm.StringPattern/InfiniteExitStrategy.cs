namespace Axh.Optimization.GeneticAlgorithm.StringPattern
{
    using Axh.Optimization.GeneticAlgorithm.Contracts.Extensibility;
    using Axh.Optimization.GeneticAlgorithm.Contracts.Models;

    public class InfiniteExitStrategy : IExitStrategy
    {
        public bool CheckExitStrategy(GenerationState generationState)
        {
            return false;
        }
    }
}
