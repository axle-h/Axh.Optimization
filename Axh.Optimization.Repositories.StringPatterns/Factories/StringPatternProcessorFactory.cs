namespace Axh.Optimization.Repositories.StringPatterns.Factories
{
    using System;

    using Axh.Optimization.DomainModels.StringPatterns;
    using Axh.Optimization.Repositories.StringPatterns.Contracts;

    public class StringPatternProcessorFactory : IStringPatternProcessorFactory
    {
        public StringPatternProcessorResult GetStringPatternProcessorResult(StringPatternProcessor stringPatternProcessor, int generationNumber, int populationNumber)
        {
            return new StringPatternProcessorResult
                   {
                       Id = stringPatternProcessor.Id,
                       GenerationNumber = generationNumber,
                       PopulationRank = populationNumber,
                       SingleWordPatterns = stringPatternProcessor.SingleWordPatterns,
                       DoubleWordPatterns = stringPatternProcessor.DoubleWordPatterns,
                       TripleWordPatterns = stringPatternProcessor.TripleWordPatterns
                   };
        }

        public StringPatternProcessor GetStringPatternProcessor(StringPatternProcessorResult stringPatternProcessorResult)
        {
            return new StringPatternProcessor(stringPatternProcessorResult.Id, stringPatternProcessorResult.SingleWordPatterns, stringPatternProcessorResult.DoubleWordPatterns, stringPatternProcessorResult.TripleWordPatterns);
        }
    }
}
