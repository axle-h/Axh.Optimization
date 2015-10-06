namespace Axh.Optimization.GeneticAlgorithm
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Axh.Optimization.GeneticAlgorithm.Contracts;
    using Axh.Optimization.GeneticAlgorithm.Contracts.Extensibility;
    using Axh.Optimization.GeneticAlgorithm.Contracts.Models;
    using Axh.Optimization.Services.RandomService.Contracts;

    /// <summary>
    /// This spools up loads of threads but, due to it's usage pattern of IGod, is not thread safe itself.
    /// </summary>
    /// <typeparam name="TCandidate"></typeparam>
    public class GeneticAlgorithm<TCandidate> : IGeneticAlgorithm<TCandidate>
    {
        private readonly IGeneticAlgorithmStateFactory geneticAlgorithmStateFactory;

        private readonly IStateLogger<TCandidate> logger;

        private readonly IRandomService randomService;

        private readonly IGod<TCandidate> god;

        private readonly IExitStrategy exitStrategy;

        public GeneticAlgorithm(IGeneticAlgorithmStateFactory geneticAlgorithmStateFactory, IStateLogger<TCandidate> logger, IGod<TCandidate> god, IExitStrategy exitStrategy, IRandomService randomService)
        {
            this.geneticAlgorithmStateFactory = geneticAlgorithmStateFactory;
            this.logger = logger;
            this.god = god;
            this.exitStrategy = exitStrategy;
            this.randomService = randomService;
        }

        public GeneticAlgorithmResult<TCandidate> RunGeneticAlgorithm()
        {
            god.Initialize();

            // Initialize population.
            var population = Enumerable.Range(0, this.god.PopulationSize).Select(this.god.InitialSeed).ToList();
            var generationNumber = 0;

            while (true)
            {

                this.logger.LogStartGeneration(generationNumber);

                var populationFitness =
                    population.AsParallel()
                        .Select(x => new { Fitness = this.god.GetFitness(x), Chromosome = x })
                        .AsSequential()
                        .OrderByDescending(x => x.Fitness)
                        .Select((x, i) => this.GetChromosome(x.Chromosome, i, x.Fitness))
                        .ToList();

                var generationState = this.geneticAlgorithmStateFactory.GetGenerationState(generationNumber, populationFitness.Select(x => x.Fitness));

                this.logger.LogGenerationResult(generationState, populationFitness.Take(10), populationFitness.Select(x => x.Candidate));

                // Check exit strategy.
                if (this.exitStrategy.CheckExitStrategy(generationState))
                {
                    return this.geneticAlgorithmStateFactory.GetGeneticAlgorithmResult(generationState, populationFitness);
                }

                var evolutionSummary = GetEvolutionElgibilitySummary(populationFitness);

                // Generate new population
                population = populationFitness.AsParallel().Select(x => this.Evolve(populationFitness, x)).ToList();

                // Remove similies
                var similies = 0;
                for (var i = 0; i < population.Count - 1; i++)
                {
                    if (this.god.IsSimilar(population[i], population[i + 1]))
                    {
                        // Mutate the lowest scoring duplicate
                        i++;
                        population[i] = this.god.Mutate(population[i]);
                        similies++;
                    }
                }

                this.logger.LogEvolutionResult(evolutionSummary, similies);

                generationNumber++;
            }
        }

        private Chromosome<TCandidate> GetRandomExcept(IList<Chromosome<TCandidate>> population, int exceptIndex)
        {
            var indexes = Enumerable.Range(0, population.Count).Except(new[] { exceptIndex }).ToArray();
            var index = this.randomService.NextInt(indexes.Length);
            return population[indexes[index]];
        }

        private static Chromosome<TCandidate> GetPreviousPercentileNeighbour(IList<Chromosome<TCandidate>> population, int index)
        {
            var percentileSize = population.Count / 1000;
            index += percentileSize;
            return population[index >= population.Count ? population.Count - 1 : index];
        }

        private static Chromosome<TCandidate> GetPreviousNeighbour(IList<Chromosome<TCandidate>> population, int index)
        {
            index--;
            return population[index < 0 ? 0 : index];
        }

        private TCandidate Evolve(IList<Chromosome<TCandidate>> population, Chromosome<TCandidate> chromosome)
        {
            var index = population.IndexOf(chromosome);
            switch (chromosome.EvolutionEligibility)
            {
                case EvolutionEligibility.Clone:
                    return this.god.Clone(chromosome);
                case EvolutionEligibility.BreedNeighbour:
                    return this.god.Breed(chromosome, GetPreviousNeighbour(population, index));
                case EvolutionEligibility.BreedPercentile:
                    return this.god.Breed(chromosome, GetPreviousPercentileNeighbour(population, index));
                case EvolutionEligibility.BreedRandom:
                    return this.god.Breed(chromosome, this.GetRandomExcept(population, index));
                case EvolutionEligibility.Mutate:
                    return this.god.Mutate(chromosome.Candidate);
                case EvolutionEligibility.None:
                    return this.god.Seed();
                default:
                    throw new Exception("Chromosome has not been assigned an EvolutionEligibility: " + chromosome.Fitness);
            }
        }

        private static IDictionary<EvolutionEligibility, int> GetEvolutionElgibilitySummary(IEnumerable<Chromosome<TCandidate>> chromosome)
        {
            return chromosome.GroupBy(x => x.EvolutionEligibility).ToDictionary(grp => grp.Key, grp => grp.Count());
        }

        private Chromosome<TCandidate> GetChromosome(TCandidate candidate, int populationPosition, double fitness)
        {
            var fitnessResultState = this.geneticAlgorithmStateFactory.GetFitnessResultState(this.god.PopulationSize, populationPosition, fitness);
            var evolutionEligibility = this.god.EvolutionStrategy.GetEvolutionEligibility(fitnessResultState) ?? EvolutionEligibility.None;

            return new Chromosome<TCandidate>(candidate, fitnessResultState, evolutionEligibility);
        }
        
    }


}
