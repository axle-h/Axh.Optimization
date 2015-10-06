namespace Axh.Optimization.Services.RandomService.Contracts
{
    using System.Collections.Generic;

    public interface IRandomService
    {
        int NextInt();

        int NextInt(int minValue, int maxValue);

        int NextInt(int maxValue);

        double NextDouble();

        IEnumerable<int> GetRandomIndexes(int collectionLength, int numberOfIndexes);

        IEnumerable<TElement> GetRandomElements<TElement>(ICollection<TElement> collection, int numberOfElements);

        IEnumerable<TElement> GetRandomElements<TElement>(IEnumerable<TElement> collection, IEnumerable<TElement> exclude, int numberOfElements);
    }
}