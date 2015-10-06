namespace Axh.Optimization.GeneticAlgorithm.StringPattern
{
    using Axh.Optimization.GeneticAlgorithm.Contracts.Extensibility;
    using Axh.Optimization.GeneticAlgorithm.Contracts.Models;

    public class EvolutionStrategy : IEvolutionStrategy
    {
        public EvolutionEligibility? GetEvolutionEligibility(FitnessResultState state)
        {
            if (state.PopulationPercentile > 99)
            {
                return EvolutionEligibility.Clone;
            }

            if (state.PopulationPercentile > 98)
            {
                return EvolutionEligibility.BreedPercentile;
            }
            
            if (state.PopulationPercentile > 90)
            {
                return EvolutionEligibility.Mutate;
            }

            if (state.PopulationPercentile > 50)
            {
                return EvolutionEligibility.BreedRandom;
            }

            return EvolutionEligibility.None;
        }
    }
}
