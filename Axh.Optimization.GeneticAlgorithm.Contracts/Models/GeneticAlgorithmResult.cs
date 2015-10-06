namespace Axh.Optimization.GeneticAlgorithm.Contracts.Models
{
    using System.Collections.Generic;

    public class GeneticAlgorithmResult<TCandidate>
    {
        public IEnumerable<TCandidate> FinalPopulation { get; set; }

        public GenerationState FinalGeneration { get; set; }
    }
}
