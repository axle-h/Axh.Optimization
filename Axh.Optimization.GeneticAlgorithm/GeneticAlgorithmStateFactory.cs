namespace Axh.Optimization.GeneticAlgorithm
{
    using System.Collections.Generic;
    using System.Linq;

    using Axh.Optimization.GeneticAlgorithm.Contracts;
    using Axh.Optimization.GeneticAlgorithm.Contracts.Models;

    public class GeneticAlgorithmStateFactory : IGeneticAlgorithmStateFactory
    {
        public GenerationState GetGenerationState(int generationNumber, IEnumerable<FitnessResultState> fitnessResults)
        {
            var scores = fitnessResults.Select(x => x.Fitness).OrderByDescending(x => x).ToArray();

            return new GenerationState
                   {
                       AverageScore = scores.Average(),
                       TopDecileAverageScore = scores.Take(scores.Length / 10).Average(),
                       MaxScore = scores.Max(),
                       MinScore = scores.Min(),
                       GenerationNumber = generationNumber
                   };
        }

        public FitnessResultState GetFitnessResultState(int populationSize, int populationPosition, double fitness)
        {
            return new FitnessResultState { PopulationPercentile = 100.0 * (populationSize - populationPosition) / populationSize, Fitness = fitness };
        }
        
        public GeneticAlgorithmResult<TCandidate> GetGeneticAlgorithmResult<TCandidate>(GenerationState state, IEnumerable<IChromosome<TCandidate>> population)
        {
            return new GeneticAlgorithmResult<TCandidate> { FinalGeneration = state, FinalPopulation = population.Select(x => x.Candidate) };
        }
    }
}
