namespace Axh.Optimization.GeneticAlgorithm.StringPattern
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    using Axh.Optimization.DomainModels.StringPatterns;
    using Axh.Optimization.GeneticAlgorithm.Contracts;
    using Axh.Optimization.GeneticAlgorithm.Contracts.Extensibility;
    using Axh.Optimization.GeneticAlgorithm.Contracts.Models;
    using Axh.Optimization.GeneticAlgorithm.StringPattern.Contracts.Repositories;
    using Axh.Optimization.Repositories.StringPatterns.Contracts;

    using log4net;

    public class StateLogger : IStateLogger<StringPatternProcessor>
    {
        private readonly ILog log;

        private readonly IGenerationStateRepository generationStateRepository;

        private readonly IStringPatternProcessorRepository stringPatternProcessorRepository;

        private readonly Stopwatch generationStopwatch;

        private Tuple<Guid, Guid, Guid> lastTop3;

        public StateLogger(ILog log, IGenerationStateRepository generationStateRepository, IStringPatternProcessorRepository stringPatternProcessorRepository)
        {
            this.log = log;
            this.generationStateRepository = generationStateRepository;
            this.stringPatternProcessorRepository = stringPatternProcessorRepository;
            this.generationStopwatch = new Stopwatch();
        }

        public void LogStartGeneration(int generationNumber)
        {
            if (this.generationStopwatch.IsRunning)
            {
                this.generationStopwatch.Stop();
                this.log.InfoFormat("Last Generation Took: {0}ms", this.generationStopwatch.ElapsedMilliseconds);
                this.generationStopwatch.Reset();
            }

            this.generationStopwatch.Start();
            this.log.Info("Starting Generation: " + generationNumber);
        }

        public void LogGenerationResult(GenerationState generationState, IEnumerable<IChromosome<StringPatternProcessor>> fittestPopulation, IEnumerable<StringPatternProcessor> candidates)
        {
            var top3Candidates = fittestPopulation.Take(3).ToArray();
            var top3Ids = top3Candidates.Select(x => x.Candidate.Id).ToArray();
            var top3 = Tuple.Create(top3Ids[0], top3Ids[1], top3Ids[2]);
            
            var generationString = string.Format(
                "GenerationNumber: {0}, Max: {1}, Min: {2}, Avg: {3}, TopAvg: {4}{5}",
                generationState.GenerationNumber,
                generationState.MaxScore,
                generationState.MinScore,
                generationState.AverageScore,
                generationState.TopDecileAverageScore,
                !top3.Equals(this.lastTop3) ? "\n" + string.Join("\n", top3Candidates.Select((x, i) => string.Format("{0:x1}: {1} - {2}", i + 1, x.Fitness.Fitness, x.Candidate.ToString()))) : null);

            this.lastTop3 = top3;

            this.log.Info(generationString);

            this.generationStateRepository.AddGenerationState(generationState);
            this.stringPatternProcessorRepository.AddStringPatternProcessors(generationState.GenerationNumber, candidates);
        }

        public void LogEvolutionResult(IDictionary<EvolutionEligibility, int> evolutionResult, int similiesRemoved)
        {
            var evolutionString = string.Join(", ", evolutionResult.OrderBy(kvp => kvp.Key).Select(kvp => string.Format("{0}: {1}", kvp.Key, kvp.Value)));
            if (similiesRemoved > 0)
            {
                evolutionString += ", Similies: " + similiesRemoved;
            }
            this.log.Info(evolutionString);
        }
    }
}
