namespace Axh.Optimization.GeneticAlgorithm.Contracts.Models
{
    public class GenerationState
    {
        public int GenerationNumber { get; set; }

        public double MaxScore { get; set; }

        public double MinScore { get; set; }

        public double AverageScore { get; set; }

        public double TopDecileAverageScore { get; set; }
    }
}
