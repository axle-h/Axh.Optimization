namespace Axh.Optimization.GeneticAlgorithm.Contracts.Extensibility
{
    
    public interface IGod<TCandidate>
    {
        void Initialize();

        int PopulationSize { get; }

        TCandidate Seed();

        /// <summary>
        /// Get intiial chromosome for population member
        /// </summary>
        /// <param name="n">Population membern position (range 0 to PopulationSize)</param>
        /// <returns>A seeded chromosome</returns>
        TCandidate InitialSeed(int n);

        double GetFitness(TCandidate candidate);

        TCandidate Breed(IChromosome<TCandidate> chromosomeX, IChromosome<TCandidate> chromosomeY);

        TCandidate Mutate(TCandidate candidate);

        TCandidate Clone(IChromosome<TCandidate> chromosome);

        bool IsSimilar(TCandidate candidate1, TCandidate candidate2);

        IEvolutionStrategy EvolutionStrategy { get; }
    }
}
