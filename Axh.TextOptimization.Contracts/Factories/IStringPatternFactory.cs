namespace Axh.TextOptimization.Contracts.Factories
{
    using Axh.Optimization.DomainModels.StringPatterns;

    public interface IStringPatternFactory
    {
        StringPattern GetStringPattern(RawText rawText);
    }
}