namespace Axh.TextOptimization.Services
{
    using System.Collections.Generic;
    using System.Linq;

    using Axh.Optimization.DomainModels.StringPatterns;
    using Axh.Optimization.GeneticAlgorithm.StringPattern.Contracts.Services;
    using Axh.Optimization.Repositories.StringPatterns.Contracts;
    using Axh.TextOptimization.Contracts.Factories;
    using Axh.TextOptimization.Contracts.Repositories;

    using log4net;

    public class StringPatternService : IStringPatternService
    {
        private readonly IRawTextRepository rawTextRepository;

        private readonly IStringPatternFactory stringPatternFactory;

        private readonly IStringPatternRepository stringPatternRepository;

        private readonly ILog log;

        public StringPatternService(IRawTextRepository rawTextRepository, IStringPatternFactory stringPatternFactory, IStringPatternRepository stringPatternRepository, ILog log)
        {
            this.rawTextRepository = rawTextRepository;
            this.stringPatternFactory = stringPatternFactory;
            this.stringPatternRepository = stringPatternRepository;
            this.log = log;
        }

        public IEnumerable<StringPattern> GetAllCachedStringPatterns()
        {
            return this.stringPatternRepository.GetAllStringPatterns();
        }

        public IEnumerable<StringPattern> UpdateCachedStringPatterns()
        {
            var allRawText = this.rawTextRepository.GetAllRawText();

            log.InfoFormat("Read {0} records.", allRawText.Count);

            var stringPatterns = allRawText.AsParallel().Select(this.stringPatternFactory.GetStringPattern).ToList();

            log.InfoFormat("Parsed {0} string patterns.", stringPatterns.Count);

            this.stringPatternRepository.CacheAllStringPatterns(stringPatterns);

            return stringPatterns;
        }
    }
}
