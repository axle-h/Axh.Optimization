namespace Axh.Optimization.GeneticAlgorithm
{
    using Axh.Optimization.GeneticAlgorithm.Contracts;
    using Axh.Optimization.GeneticAlgorithm.Contracts.Models;

    public class Chromosome<TCandidate> : IChromosome<TCandidate>
    {
        public Chromosome(TCandidate candidate, FitnessResultState fitness, EvolutionEligibility evolutionEligibility)
        {
            this.Candidate = candidate;
            this.Fitness = fitness;
            this.EvolutionEligibility = evolutionEligibility;
        }

        public TCandidate Candidate { get; set; }

        public FitnessResultState Fitness { get; set; }

        public EvolutionEligibility EvolutionEligibility { get; set; }
    }
}
