namespace Axh.Optimization.GeneticAlgorithm.StringPattern.Contracts.Repositories
{
    using Axh.Optimization.GeneticAlgorithm.Contracts.Models;

    public interface IGenerationStateRepository
    {
        void AddGenerationState(GenerationState generationState);
    }
}