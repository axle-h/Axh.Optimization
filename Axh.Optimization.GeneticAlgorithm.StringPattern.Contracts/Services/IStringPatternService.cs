namespace Axh.Optimization.GeneticAlgorithm.StringPattern.Contracts.Services
{
    using System.Collections.Generic;

    using Axh.Optimization.DomainModels.StringPatterns;

    public interface IStringPatternService
    {
        IEnumerable<StringPattern> GetAllCachedStringPatterns();

        IEnumerable<StringPattern> UpdateCachedStringPatterns();
    }
}