namespace Axh.Optimization.Repositories.StringPatterns.Contracts
{
    using System.Collections.Generic;

    using Axh.Optimization.DomainModels.StringPatterns;

    public interface IStringPatternRepository
    {
        IEnumerable<StringPattern> GetAllStringPatterns();

        void CacheAllStringPatterns(IEnumerable<StringPattern> patterns);
    }
}