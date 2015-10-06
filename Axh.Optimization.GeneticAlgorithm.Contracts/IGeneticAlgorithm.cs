namespace Axh.Optimization.GeneticAlgorithm.Contracts
{
    using Axh.Optimization.GeneticAlgorithm.Contracts.Models;

    public interface IGeneticAlgorithm<TCandidate>
    {
        GeneticAlgorithmResult<TCandidate> RunGeneticAlgorithm();
    }
}