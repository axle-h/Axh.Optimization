namespace Axh.Optimization.GeneticAlgorithm.Contracts.Extensibility
{
    using System.Collections.Generic;

    using Axh.Optimization.GeneticAlgorithm.Contracts.Models;

    public interface IStateLogger<in TCandidate>
    {
        void LogStartGeneration(int generationNumber);

        void LogGenerationResult(GenerationState generationState, IEnumerable<IChromosome<TCandidate>> fittestPopulation, IEnumerable<TCandidate> candidates);

        void LogEvolutionResult(IDictionary<EvolutionEligibility, int> evolutionResult, int similiesRemoved);

    }
}
