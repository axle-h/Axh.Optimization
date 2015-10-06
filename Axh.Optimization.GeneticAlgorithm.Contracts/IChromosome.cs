namespace Axh.Optimization.GeneticAlgorithm.Contracts
{
    using Axh.Optimization.GeneticAlgorithm.Contracts.Models;

    public interface IChromosome<out TCandidate>
    {
        TCandidate Candidate { get; }

        FitnessResultState Fitness { get; }

        EvolutionEligibility EvolutionEligibility { get; }
    }
}
