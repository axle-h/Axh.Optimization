namespace Axh.Optimization.GeneticAlgorithm.StringPattern
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Axh.Optimization.GeneticAlgorithm.Contracts.Extensibility;
    using Axh.Optimization.GeneticAlgorithm.Contracts.Models;

    public class SignificantThresholdExitStrategy : IExitStrategy
    {
        private const double SignificantThreshold = 0.01;

        private const int PreviousGenerationsToKeep = 10;

        private readonly IDictionary<int, double> previousGenerationsScores;

        public SignificantThresholdExitStrategy()
        {
            this.previousGenerationsScores = new Dictionary<int, double>();
        }

        public bool CheckExitStrategy(GenerationState generationState)
        {
            this.previousGenerationsScores.Add(generationState.GenerationNumber, generationState.TopDecileAverageScore);
            if (this.previousGenerationsScores.Count <= PreviousGenerationsToKeep)
            {
                // Don't exit until we have enough generations.
                return false;
            }

            // Remove oldest generation.
            var minimumGeneration = this.previousGenerationsScores.Keys.Min();
            this.previousGenerationsScores.Remove(minimumGeneration);

            // Exit if score hasn't changed siginificantly in previous generations.
            return
                this.previousGenerationsScores.OrderBy(x => x.Key)
                    .Skip(1)
                    .All(generation => !(Math.Abs(this.previousGenerationsScores[generation.Key - 1] - generation.Value) > SignificantThreshold));
        }
    }
}
