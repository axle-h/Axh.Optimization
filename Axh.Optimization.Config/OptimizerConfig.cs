namespace Axh.Optimization.Config
{
    using System;
    using System.Configuration;

    using Axh.Optimization.Config.Contracts;

    public class OptimizerConfig : IOptimizerConfig
    {
        private const string InstanceIdKey = "GeneticAlgorithm_InstanceId";
        private readonly string instanceId;

        private const string PopulationSizeValueKey = "GeneticAlgorithm_PopulationSizeValue";
        private const int DefaultPopulationSizeValue = 100;
        private readonly int populationSizeValue;

        private const string MinimumSingleWordLengthKey = "GeneticAlgorithm_MinimumSingleWordLength";
        private const int DefaultMinimumSingleWordLength = 4;
        private readonly int minimumSingleWordLength;

        private const string MaxInitialScoreKey = "GeneticAlgorithm_MaxInitialScore";
        private const double DefaultMaxInitialScore = 10.0;
        private readonly double maxInitialScore;

        private const string ExcludeSingleWordsKey = "GeneticAlgorithm_ExcludeSingleWords";
        private readonly string[] excludeSingleWords;

        private const string MinimumWordUsagesKey = "GeneticAlgorithm_MinimumWordUsages";
        private const int DefaultMinimumSingleWordUsages = 10;
        private const int DefaultMinimumDoubleWordUsages = 5;
        private const int DefaultMinimumTripleWordUsages = 5;
        private readonly int minimumSingleWordUsages;
        private readonly int minimumDoubleWordUsages;
        private readonly int minimumTripleWordUsages;

        private const string MaxWordsKey = "GeneticAlgorithm_MaxWords";
        private const int DefaultMaxSingleWords = 15;
        private const int DefaultMaxDoubleWords = 10;
        private const int DefaultMaxTripleWords = 5;
        private readonly int maxDoubleWords;
        private readonly int maxSingleWords;
        private readonly int maxTripleWords;


        private const string MaxWordsToMutateKey = "GeneticAlgorithm_MaxWordsToMutate";
        private const int DefaultMaxSingleWordsToMutate = 5;
        private const int DefaultMaxDoubleWordsToMutate = 3;
        private const int DefaultMaxTripleWordsToMutate = 3;
        private readonly int maxSingleWordsToMutate;
        private readonly int maxDoubleWordsToMutate;
        private readonly int maxTripleWordsToMutate;

        private const string MaxWordsToRemoveKey = "GeneticAlgorithm_MaxWordsToRemove";
        private const int DefaultMaxSingleWordsToRemove = 5;
        private const int DefaultMaxDoubleWordsToRemove = 3;
        private const int DefaultMaxTripleWordsToRemove = 3;
        private readonly int maxSingleWordsToRemove;
        private readonly int maxDoubleWordsToRemove;
        private readonly int maxTripleWordsToRemove;

        private const string MaxWordsToAddKey = "GeneticAlgorithm_MaxWordsToAdd";
        private const int DefaultMaxSingleWordsToAdd = 5;
        private const int DefaultMaxDoubleWordsToAdd = 3;
        private const int DefaultMaxTripleWordsToAdd = 3;
        private readonly int maxSingleWordsToAdd;
        private readonly int maxDoubleWordsToAdd;
        private readonly int maxTripleWordsToAdd;

        private const string MaxScoreAdjustmentKey = "GeneticAlgorithm_MaxScoreAdjustment";
        private const double DefaultMaxScoreAdjustment = 5.0;
        private readonly double maxScoreAdjustment;


        public OptimizerConfig()
        {
            var instanceIdConfigValue = ConfigurationManager.AppSettings[InstanceIdKey];
            this.instanceId = string.IsNullOrWhiteSpace(instanceIdConfigValue) ? "DefaultInstance" : instanceIdConfigValue;

            this.populationSizeValue = ParseConfigInteger(PopulationSizeValueKey, DefaultPopulationSizeValue);
            this.minimumSingleWordLength = ParseConfigInteger(MinimumSingleWordLengthKey, DefaultMinimumSingleWordLength);
            this.maxInitialScore = ParseConfigDouble(MaxInitialScoreKey, DefaultMaxInitialScore);

            var excludeSingleWordsConfigValue = ConfigurationManager.AppSettings[ExcludeSingleWordsKey];
            this.excludeSingleWords = string.IsNullOrWhiteSpace(excludeSingleWordsConfigValue)
                ? new string[0]
                : excludeSingleWordsConfigValue.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            ParseConfigIntegers(
                MinimumWordUsagesKey,
                DefaultMinimumSingleWordUsages,
                DefaultMinimumDoubleWordUsages,
                DefaultMinimumTripleWordUsages,
                out this.minimumSingleWordUsages,
                out this.minimumDoubleWordUsages,
                out this.minimumTripleWordUsages);

            ParseConfigIntegers(
                MaxWordsKey,
                DefaultMaxSingleWords,
                DefaultMaxDoubleWords,
                DefaultMaxTripleWords,
                out this.maxSingleWords,
                out this.maxDoubleWords,
                out this.maxTripleWords);

            ParseConfigIntegers(
                MaxWordsToMutateKey,
                DefaultMaxSingleWordsToMutate,
                DefaultMaxDoubleWordsToMutate,
                DefaultMaxTripleWordsToMutate,
                out this.maxSingleWordsToMutate,
                out this.maxDoubleWordsToMutate,
                out this.maxTripleWordsToMutate);

            ParseConfigIntegers(
                MaxWordsToRemoveKey,
                DefaultMaxSingleWordsToRemove,
                DefaultMaxDoubleWordsToRemove,
                DefaultMaxTripleWordsToRemove,
                out this.maxSingleWordsToRemove,
                out this.maxDoubleWordsToRemove,
                out this.maxTripleWordsToRemove);

            ParseConfigIntegers(
                MaxWordsToAddKey,
                DefaultMaxSingleWordsToAdd,
                DefaultMaxDoubleWordsToAdd,
                DefaultMaxTripleWordsToAdd,
                out this.maxSingleWordsToAdd,
                out this.maxDoubleWordsToAdd,
                out this.maxTripleWordsToAdd);

            this.maxScoreAdjustment = ParseConfigDouble(MaxScoreAdjustmentKey, DefaultMaxScoreAdjustment);
        }

        public string InstanceId
        {
            get
            {
                return this.instanceId;
            }
        }

        public string MongoConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["MongoDB_Local"].ConnectionString;
            }
        }

        public string MongoDatabasePrefix
        {
            get
            {
                return ConfigurationManager.AppSettings["Mongo_DatabasePrefix"];
            }
        }

        public int PopulationSizeValue
        {
            get
            {
                return this.populationSizeValue;
            }
        }

        public int MinimumSingleWordUsages
        {
            get
            {
                return this.minimumSingleWordUsages;
            }
        }

        public int MinimumDoubleWordUsages
        {
            get
            {
                return this.minimumDoubleWordUsages;
            }
        }

        public int MinimumTripleWordUsages
        {
            get
            {
                return this.minimumTripleWordUsages;
            }
        }

        public int MinimumSingleWordLength
        {
            get
            {
                return this.minimumSingleWordLength;
            }
        }

        public double MaxInitialScore
        {
            get
            {
                return this.maxInitialScore;
            }
        }

        public int MaxSingleWords
        {
            get
            {
                return this.maxSingleWords;
            }
        }

        public int MaxDoubleWords
        {
            get
            {
                return this.maxDoubleWords;
            }
        }

        public int MaxTripleWords
        {
            get
            {
                return this.maxTripleWords;
            }
        }

        public string[] ExcludeSingleWords
        {
            get
            {
                return this.excludeSingleWords;
            }
        }

        public int MaxSingleWordsToMutate
        {
            get
            {
                return this.maxSingleWordsToMutate;
            }
        }

        public int MaxDoubleWordsToMutate
        {
            get
            {
                return this.maxDoubleWordsToMutate;
            }
        }

        public int MaxTripleWordsToMutate
        {
            get
            {
                return this.maxTripleWordsToMutate;
            }
        }

        public int MaxSingleWordsToRemove
        {
            get
            {
                return this.maxSingleWordsToRemove;
            }
        }

        public int MaxDoubleWordsToRemove
        {
            get
            {
                return this.maxDoubleWordsToRemove;
            }
        }

        public int MaxTripleWordsToRemove
        {
            get
            {
                return this.maxTripleWordsToRemove;
            }
        }

        public int MaxSingleWordsToAdd
        {
            get
            {
                return this.maxSingleWordsToAdd;
            }
        }

        public int MaxDoubleWordsToAdd
        {
            get
            {
                return this.maxDoubleWordsToAdd;
            }
        }

        public int MaxTripleWordsToAdd
        {
            get
            {
                return this.maxTripleWordsToAdd;
            }
        }

        public double MaxScoreAdjustment
        {
            get
            {
                return this.maxScoreAdjustment;
            }
        }

        private static int ParseConfigInteger(string keyName, int defaultValue)
        {
            var configValue = ConfigurationManager.AppSettings[keyName];
            int value;
            return int.TryParse(configValue, out value) ? value : defaultValue;
        }

        private static bool ParseConfigIntegers(string keyName, int defaultValue1, int defaultValue2, int defaultValue3, out int value1, out int value2, out int value3)
        {
            var configValue = ConfigurationManager.AppSettings[keyName];

            if (string.IsNullOrEmpty(configValue))
            {
                value1 = defaultValue1;
                value2 = defaultValue2;
                value3 = defaultValue3;
                return false;
            }

            var split = configValue.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            if (split.Length != 3)
            {
                value1 = defaultValue1;
                value2 = defaultValue2;
                value3 = defaultValue3;
                return false;
            }

            return int.TryParse(split[0], out value1) & int.TryParse(split[1], out value2) & int.TryParse(split[2], out value3);
        }

        private static double ParseConfigDouble(string keyName, double defaultValue)
        {
            var configValue = ConfigurationManager.AppSettings[keyName];
            double value;
            return double.TryParse(configValue, out value) ? value : defaultValue;
        }
    }
}
