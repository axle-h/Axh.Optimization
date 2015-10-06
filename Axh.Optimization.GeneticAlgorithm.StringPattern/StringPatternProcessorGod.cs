namespace Axh.TextOptimization.GeneticAlgorithm
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;

    using Axh.Optimization.Config.Contracts;
    using Axh.Optimization.DomainModels.StringPatterns;
    using Axh.Optimization.GeneticAlgorithm.Contracts;
    using Axh.Optimization.GeneticAlgorithm.Contracts.Extensibility;
    using Axh.Optimization.GeneticAlgorithm.StringPattern.Contracts.Services;
    using Axh.Optimization.Services.RandomService.Contracts;

    public class StringPatternProcessorGod : IGod<StringPatternProcessor>
    {
        private readonly IEvolutionStrategy evolutionStrategy;

        private readonly IOptimizerConfig optimizerConfig;

        private readonly IRandomService randomService;

        private readonly IStringPatternService stringPatternService;

        // Should be ok letting this leak
        private readonly ConcurrentDictionary<Guid, double> scoreCache;

        private ICollection<string> wordList;

        private ICollection<TwoWords> twoWordList;

        private ICollection<ThreeWords> threeWordList;

        private ICollection<StringPattern> testStringPatterns;

        private bool isInitialized;

        public StringPatternProcessorGod(IEvolutionStrategy evolutionStragey, IStringPatternService stringPatternService, IOptimizerConfig optimizerConfig, IRandomService randomService)
        {
            this.isInitialized = false;
            this.evolutionStrategy = evolutionStragey;
            this.stringPatternService = stringPatternService;
            this.optimizerConfig = optimizerConfig;
            this.randomService = randomService;
            this.scoreCache = new ConcurrentDictionary<Guid, double>();
        }

        public void Initialize()
        {
            if (this.isInitialized)
            {
                return;
            }
            this.isInitialized = true;

            var stringPatterns = stringPatternService.GetAllCachedStringPatterns().OrderByDescending(x => x.KnownScore).ToList();

            if (!stringPatterns.Any())
            {
                throw new Exception("No data to optimize.");
            }

            //var topStringPatterns = stringPatterns.Take(stringPatterns.Count / 10);
            //var bottomStringPatterns = stringPatterns.OrderBy(x => x.KnownScore).Take(stringPatterns.Count / 10);
            //this.testStringPatterns = topStringPatterns.Concat(bottomStringPatterns).ToList();
            this.testStringPatterns = stringPatterns;

            this.wordList =
                this.testStringPatterns.AsParallel()
                    .SelectMany(x => x.Words)
                    .GroupBy(x => x)
                    .Where(
                        x =>
                            x.Key.Length > optimizerConfig.MinimumSingleWordLength && !optimizerConfig.ExcludeSingleWords.Contains(x.Key)
                            && x.Count() >= optimizerConfig.MinimumSingleWordUsages)
                    .Select(x => x.Key)
                    .ToList();

            this.twoWordList = this.testStringPatterns.AsParallel().SelectMany(x => x.DoubleWords).GroupBy(x => x).Where(x => x.Count() > optimizerConfig.MinimumDoubleWordUsages).Select(x => x.Key).ToList();
            this.threeWordList = this.testStringPatterns.AsParallel().SelectMany(x => x.TripleWords).GroupBy(x => x).Where(x => x.Count() > optimizerConfig.MinimumTripleWordUsages).Select(x => x.Key).ToList();
        }
        
        public int PopulationSize
        {
            get
            {
                return this.optimizerConfig.PopulationSizeValue;
            }
        }

        public StringPatternProcessor Breed(IChromosome<StringPatternProcessor> chromosomeX, IChromosome<StringPatternProcessor> chromosomeY)
        {
            var newSingleWords = this.BreedDictionaries(chromosomeX.Candidate.SingleWordPatterns, chromosomeY.Candidate.SingleWordPatterns);
            var newDoubleWords = this.BreedDictionaries(chromosomeX.Candidate.DoubleWordPatterns, chromosomeY.Candidate.DoubleWordPatterns);
            var newTripleWords = this.BreedDictionaries(chromosomeX.Candidate.TripleWordPatterns, chromosomeY.Candidate.TripleWordPatterns);

            // Take the parents id because mutation creates a new one anyway
            var result = new StringPatternProcessor(chromosomeX.Candidate.Id, newSingleWords, newDoubleWords, newTripleWords);
            return this.Mutate(0.5, result);
        }

        public StringPatternProcessor Mutate(StringPatternProcessor candidate)
        {
            return this.Mutate(1, candidate);
        }

        public StringPatternProcessor Clone(IChromosome<StringPatternProcessor> chromosome)
        {
            // Keep track of this score as it will very likely be seen again.
            if (!this.scoreCache.ContainsKey(chromosome.Candidate.Id))
            {
                this.scoreCache.AddOrUpdate(chromosome.Candidate.Id, chromosome.Fitness.Fitness, (key, value) => chromosome.Fitness.Fitness);
            }
            

            // Clone everything.
            var singleWords = Clone(chromosome.Candidate.SingleWordPatterns);
            var doubleWords = Clone(chromosome.Candidate.DoubleWordPatterns);
            var tripleWords = Clone(chromosome.Candidate.TripleWordPatterns);

            return new StringPatternProcessor(chromosome.Candidate.Id, singleWords, doubleWords, tripleWords);
        }

        public bool IsSimilar(StringPatternProcessor candidate1, StringPatternProcessor candidate2)
        {
            var singleWordsSimilar = this.IsSimilar(candidate1.SingleWordPatterns, candidate2.SingleWordPatterns);
            var doubleWordsSimilar = this.IsSimilar(candidate1.DoubleWordPatterns, candidate2.DoubleWordPatterns);

            if (singleWordsSimilar && doubleWordsSimilar)
            {
                return true;
            }

            var tripleWordsSimilar = this.IsSimilar(candidate1.TripleWordPatterns, candidate2.TripleWordPatterns);

            return (singleWordsSimilar ? 1 : 0) + (doubleWordsSimilar ? 1 : 0) + (tripleWordsSimilar ? 1 : 0) > 1;

        }

        private bool IsSimilar<TKey>(IDictionary<TKey, double> dictionary1, IDictionary<TKey, double> dictionary2)
        {
            if (!dictionary1.Any() && !dictionary2.Any())
            {
                return true;
            }

            if (!dictionary1.Any() || !dictionary2.Any())
            {
                return false;
            }

            var similies = dictionary1.Join(dictionary2, c1 => c1.Key, c2 => c2.Key, (c1, c2) => Math.Abs(c1.Value - c2.Value)).Count(x => x < this.optimizerConfig.MaxScoreAdjustment);
            var disSimilies = dictionary1.Where(x => !dictionary2.ContainsKey(x.Key)).Concat(dictionary2.Where(x => !dictionary1.ContainsKey(x.Key))).Count();
            return similies >= disSimilies * 0.9;
        }

        private StringPatternProcessor Mutate(double mutationFactor, StringPatternProcessor candidate)
        {
            // Clone everything.
            var singleWords = Clone(candidate.SingleWordPatterns);
            var doubleWords = Clone(candidate.DoubleWordPatterns);
            var tripleWords = Clone(candidate.TripleWordPatterns);

            // Bump some scores around
            this.AdjustScores(singleWords, this.optimizerConfig.MaxSingleWordsToMutate, mutationFactor);
            this.AdjustScores(doubleWords, this.optimizerConfig.MaxDoubleWordsToMutate, mutationFactor);
            this.AdjustScores(tripleWords, this.optimizerConfig.MaxTripleWordsToMutate, mutationFactor);

            // Remove some lower scoring words
            this.RemoveElements(singleWords, this.optimizerConfig.MaxSingleWordsToRemove, mutationFactor);
            this.RemoveElements(doubleWords, this.optimizerConfig.MaxDoubleWordsToRemove, mutationFactor);
            this.RemoveElements(tripleWords, this.optimizerConfig.MaxTripleWordsToRemove, mutationFactor);

            // Add some new scoring words
            this.AddNewElements(singleWords, this.wordList, this.optimizerConfig.MaxSingleWordsToAdd, mutationFactor);
            this.AddNewElements(doubleWords, this.twoWordList, this.optimizerConfig.MaxDoubleWordsToAdd, mutationFactor);
            this.AddNewElements(tripleWords, this.threeWordList, this.optimizerConfig.MaxTripleWordsToAdd, mutationFactor);

            return new StringPatternProcessor(Guid.NewGuid(), singleWords, doubleWords, tripleWords);
        }

        public double GetFitness(StringPatternProcessor candidate)
        {
            // Check cache first
            double cachedScore;
            if (this.scoreCache.TryGetValue(candidate.Id, out cachedScore))
            {
                return cachedScore;
            }

            // The average absolute difference between the known score and the score returned by the processor
            // Smaller differences need higher scores to take the difference from an arbitary value
            var differenceScore = 10 * this.testStringPatterns.Average(x => 0 - Math.Abs(x.KnownScore - candidate.ProcessPatterns(x.Words, x.DoubleWords, x.TripleWords)));
            var complexityScore = 0 - candidate.SingleWordPatterns.Count - 2 * candidate.DoubleWordPatterns.Count - 3 * candidate.TripleWordPatterns.Count;

            var singleWords = candidate.SingleWordPatterns.Keys;
            var doubleWords = candidate.DoubleWordPatterns.Keys.SelectMany(x => new[] { x.Word1, x.Word2 });
            var tripleWords = candidate.TripleWordPatterns.Keys.SelectMany(x => new[] { x.Word1, x.Word2, x.Word3 });
            var duplicationScore = 0 - singleWords.Concat(doubleWords).Concat(tripleWords).GroupBy(x => x).Count(x => x.Count() > 1);

            return differenceScore + complexityScore + duplicationScore;
        }

        public StringPatternProcessor Seed()
        {
            var numberOfSingleWords = this.randomService.NextInt(1, this.optimizerConfig.MaxSingleWords);
            var numberOfDoubleWords = this.randomService.NextInt(1, this.optimizerConfig.MaxDoubleWords);
            var numberOfTripleWords = this.randomService.NextInt(1, this.optimizerConfig.MaxTripleWords);

            var randomWords = this.randomService.GetRandomElements(this.wordList, numberOfSingleWords).ToDictionary(x => x, x => this.GetInitialScore());
            var randomTwoWords = this.randomService.GetRandomElements(this.twoWordList, numberOfDoubleWords).ToDictionary(x => x, x => this.GetInitialScore());
            var randomThreeWords = this.randomService.GetRandomElements(this.threeWordList, numberOfTripleWords).ToDictionary(x => x, x => this.GetInitialScore());

            return new StringPatternProcessor(Guid.NewGuid(), randomWords, randomTwoWords, randomThreeWords);
        }

        public StringPatternProcessor InitialSeed(int n)
        {
            return this.Seed();
        }

        public IEvolutionStrategy EvolutionStrategy
        {
            get
            {
                return this.evolutionStrategy;
            }
        }

        private IDictionary<TKey, double> BreedDictionaries<TKey>(ICollection<KeyValuePair<TKey, double>> dictionary1, ICollection<KeyValuePair<TKey, double>> dictionary2)
        {
            var length = (int)Math.Round(this.randomService.NextDouble() * (dictionary1.Count + dictionary2.Count));
            return
                dictionary1.Concat(dictionary2)
                    .GroupBy(x => x.Key)
                    .Select(x => new KeyValuePair<TKey, double>(x.Key, x.Average(y => y.Value)))
                    .OrderBy(x => this.randomService.NextDouble())
                    .Take(length == 0 ? 1 : length)
                    .ToDictionary(x => x.Key, x => x.Value);
        }

        private double GetInitialScore()
        {
            return this.GetRandomOriginValue(this.optimizerConfig.MaxInitialScore);
        }

        private double GetAdjustment()
        {
            return this.GetRandomOriginValue(this.optimizerConfig.MaxScoreAdjustment);
        }

        private double GetRandomOriginValue(double maxValue)
        {
            return Math.Round(this.randomService.NextDouble() * maxValue * 2.0 - maxValue, 2);
        }

        private int GetMutationMax(int configMax, double mutationFactor)
        {
            var max = (int)Math.Round(configMax * mutationFactor);
            var newMax = max == 0 ? 1 : max;
            return this.randomService.NextInt(newMax + 1);
        }

        private void AddNewElements<TKey>(IDictionary<TKey, double> dictionary, IEnumerable<TKey> list, int maxNumberOfElementsToAdd, double mutationFactor)
        {
            var numberOfElements = GetMutationMax(maxNumberOfElementsToAdd, mutationFactor);

            foreach (var key in this.randomService.GetRandomElements(list, dictionary.Keys, numberOfElements).ToArray())
            {
                dictionary.Add(key, this.GetAdjustment());
            }
        }

        private void RemoveElements<TKey>(IDictionary<TKey, double> dictionary, int maxNumberOfElementsToRemove, double mutationFactor)
        {
            // Don't remove the last element
            if (dictionary.Count <= 1)
            {
                return;
            }

            var numberOfElements = GetMutationMax(maxNumberOfElementsToRemove, mutationFactor);

            var keys = dictionary.Keys;
            foreach (var keyToRemove in this.randomService.GetRandomIndexes(dictionary.Count, numberOfElements).Select(keys.ElementAt))
            {
                dictionary.Remove(keyToRemove);
            }
        }

        private void AdjustScores<TKey>(IDictionary<TKey, double> dictionary, int maxNumberOfScoresToAdjust, double mutationFactor)
        {
            var numberOfElements = GetMutationMax(maxNumberOfScoresToAdjust, mutationFactor);

            var keys = dictionary.Keys;
            foreach (var index in this.randomService.GetRandomIndexes(keys.Count, numberOfElements))
            {
                dictionary[keys.ElementAt(index)] += this.GetAdjustment();
            }
        }

        private static IDictionary<TKey, double> Clone<TKey>(IEnumerable<KeyValuePair<TKey, double>> dictionary)
        {
            // Shallow cloned values, but we always have primitives so ok.
            return dictionary.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }
    }
}
