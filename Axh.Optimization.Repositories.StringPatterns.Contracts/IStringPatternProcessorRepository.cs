namespace Axh.Optimization.Repositories.StringPatterns.Contracts
{
    using System.Collections.Generic;

    using Axh.Optimization.DomainModels.StringPatterns;

    public interface IStringPatternProcessorRepository
    {
        void AddStringPatternProcessors(int generationNumber, IEnumerable<StringPatternProcessor> stringPatternProcessors);

        IEnumerable<StringPatternProcessor> GetStringPatternProcessors(int generationNumber);
    }
}