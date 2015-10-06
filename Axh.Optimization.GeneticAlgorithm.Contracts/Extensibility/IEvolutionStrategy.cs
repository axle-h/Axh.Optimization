namespace Axh.Optimization.GeneticAlgorithm.Contracts.Extensibility
{
    using Axh.Optimization.GeneticAlgorithm.Contracts.Models;

    public interface IEvolutionStrategy
    {
        EvolutionEligibility? GetEvolutionEligibility(FitnessResultState state);
    }
}
