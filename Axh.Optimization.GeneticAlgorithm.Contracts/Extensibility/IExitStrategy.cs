namespace Axh.Optimization.GeneticAlgorithm.Contracts.Extensibility
{
    using Axh.Optimization.GeneticAlgorithm.Contracts.Models;

    public interface IExitStrategy
    {
        bool CheckExitStrategy(GenerationState generationState);
    }
}
