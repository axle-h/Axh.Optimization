namespace Axh.Optimization.Repositories.StringPatterns.Contracts
{
    using Axh.Optimization.DomainModels.StringPatterns;

    public interface IStringPatternProcessorFactory
    {
        StringPatternProcessorResult GetStringPatternProcessorResult(StringPatternProcessor stringPatternProcessor, int generationNumber, int populationNumber);

        StringPatternProcessor GetStringPatternProcessor(StringPatternProcessorResult stringPatternProcessorResult);
    }
}