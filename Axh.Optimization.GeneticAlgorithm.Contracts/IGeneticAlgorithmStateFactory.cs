namespace Axh.Optimization.GeneticAlgorithm.Contracts
{
    using System.Collections.Generic;

    using Axh.Optimization.GeneticAlgorithm.Contracts.Models;

    public interface IGeneticAlgorithmStateFactory
    {
        GenerationState GetGenerationState(int generationNumber, IEnumerable<FitnessResultState> fitnessResults);

        FitnessResultState GetFitnessResultState(int populationSize, int populationPosition, double fitness);

        GeneticAlgorithmResult<TCandidate> GetGeneticAlgorithmResult<TCandidate>(GenerationState state, IEnumerable<IChromosome<TCandidate>> population);
    }
}