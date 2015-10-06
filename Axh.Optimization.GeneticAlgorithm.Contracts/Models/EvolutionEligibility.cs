namespace Axh.Optimization.GeneticAlgorithm.Contracts.Models
{
    public enum EvolutionEligibility
    {
        None = 0,
        Clone = 1,
        BreedNeighbour = 2,
        BreedPercentile = 3,
        BreedRandom = 4,
        Mutate = 5
    }
}
