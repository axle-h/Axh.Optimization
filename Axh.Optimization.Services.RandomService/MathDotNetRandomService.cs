namespace Axh.Optimization.Services.RandomService
{
    using System.Collections.Generic;
    using System.Linq;

    using Axh.Optimization.Services.RandomService.Contracts;

    using MathNet.Numerics.Random;

    public class MathDotNetRandomService : IRandomService
    {
        private readonly SystemRandomSource random;

        public MathDotNetRandomService()
        {
            random = new SystemRandomSource(true);
        }

        public int NextInt()
        {
            return random.Next();
        }

        public int NextInt(int minValue, int maxValue)
        {
            return random.Next(minValue, maxValue);
        }

        public int NextInt(int maxValue)
        {
            return random.Next(maxValue);
        }

        public double NextDouble()
        {
            return random.NextDouble();
        }

        public IEnumerable<int> GetRandomIndexes(int collectionLength, int numberOfIndexes)
        {
            if (collectionLength == 0 || numberOfIndexes == 0)
            {
                return Enumerable.Empty<int>();
            }

            if (collectionLength <= numberOfIndexes)
            {
                return Enumerable.Range(0, collectionLength).OrderByDescending(x => x).ToArray();
            }

            return Enumerable.Range(0, collectionLength).OrderBy(x => random.Next()).Take(numberOfIndexes).OrderByDescending(x => x).ToArray();
        }

        public IEnumerable<TElement> GetRandomElements<TElement>(ICollection<TElement> collection, int numberOfElements)
        {
            var indexes = GetRandomIndexes(collection.Count, numberOfElements);
            return indexes.Select(collection.ElementAt).ToArray();
        }

        public IEnumerable<TElement> GetRandomElements<TElement>(IEnumerable<TElement> collection, IEnumerable<TElement> exclude, int numberOfElements)
        {
            return GetRandomElements(collection.Except(exclude).ToList(), numberOfElements);
        }
    }
}
