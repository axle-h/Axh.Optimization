namespace Axh.Optimization.Config.Contracts
{
    public interface IOptimizerConfig
    {
        string InstanceId { get; }

        string MongoConnectionString { get; }

        string MongoDatabasePrefix { get; }

        int PopulationSizeValue { get; }

        int MinimumSingleWordUsages { get; }

        int MinimumDoubleWordUsages { get; }

        int MinimumTripleWordUsages { get; }

        int MinimumSingleWordLength { get; }

        double MaxInitialScore { get; }

        int MaxSingleWords { get; }

        int MaxDoubleWords { get; }

        int MaxTripleWords { get; }

        string[] ExcludeSingleWords { get; }

        int MaxSingleWordsToMutate { get; }

        int MaxDoubleWordsToMutate { get; }

        int MaxTripleWordsToMutate { get; }

        int MaxSingleWordsToRemove { get; }

        int MaxDoubleWordsToRemove { get; }

        int MaxTripleWordsToRemove { get; }

        int MaxSingleWordsToAdd { get; }

        int MaxDoubleWordsToAdd { get; }

        int MaxTripleWordsToAdd { get; }

        double MaxScoreAdjustment { get; }
    }
}