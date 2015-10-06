namespace Axh.Optimization.GeneticAlgorithm.Contracts.Models
{
    public class FitnessResultState
    {
        public double PopulationPercentile { get; set; }

        public double Fitness { get; set; }

        public override string ToString()
        {
            return string.Format("PopulationPercentile: {0}, Fitness: {1}", this.PopulationPercentile, this.Fitness);
        }
    }
}
