namespace Axh.TextOptimization
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Text.RegularExpressions;

    using Axh.Optimization.DomainModels.StringPatterns;
    using Axh.Optimization.GeneticAlgorithm.Contracts;
    using Axh.Optimization.GeneticAlgorithm.StringPattern.Contracts.Services;
    using Axh.TextOptimization.Contracts;
    using Axh.TextOptimization.Contracts.Config;
    using Axh.TextOptimization.Contracts.Repositories;

    using log4net;

    public class TextOptimization : ITextOptimization
    {
        private static readonly string[] ExcludeGoodWords =
        {
            "the", "and", "a", "to", "was", "is", "of", "in", "we", "for", "hotel", "it", "with", "you", "are", "were", "but", "all", "from", "on", "i", "at",
            "as", "there", "this", "have", "br", "not", "which", "be", "so", "our", "had", "if", "that", "an", "back", "also", "my", "has", "out", "only", "they",
            "staff", "food", "room", "rooms", "one", "get", "its", "other", "small", "or", "week","by","day", "do", "every","apartments", "about",
            "minutes", "night", "up", "bus", "choice", "2", "reception", "going", "3", "apartment", "bar", "bars", "can", "away", "children", "your", "outside", "no", "first", "both", "always", "when",
            "resort", "main", "little", "what", "want", "each", "year", "quiet", "kids", "service", "restaurants", "us", "breakfast",
            "than", "restaurant", "airport", "taxi", "into", "adults", "went", "english", "too", "facilities", "mins", "around", "enough", "two", "down", "people", "some", "found", "shops", "say", "bit", "next", "short", "pools",
            "evening", "did", "balcony", "towels", "4", "could", "them", "booked", "after", "station", "didnt", "far", "times", "although", "over", "right", "family", "off",
            "few", "size", "full", "go", "entertainment", "side", "old", "these", "buffet", "water", "here", "then", "close",  "bathroom", "tv", "minute", "marina", "looking", "bed", "better",
            "got", "been", "situated", "puerto", "through", "overall", "dining", "hrefguidespainbenidormdestinationaspx", "titlebenidorm", "road", "guests", "including", "swimming", "door",
            "12", "made", "wait", "during", "booking", "need", "beds", "local", "before", "find", "another", "where", "eat"
        };

        private readonly IStringPatternService stringPatternService;

        private readonly IGeneticAlgorithm<StringPatternProcessor> geneticAlgorithm;

        private readonly IRawTextRepository rawTextRepository;

        private readonly ILog log;

        private readonly ITextOptimizationConfig textOptimizationConfig;

        public TextOptimization(IStringPatternService stringPatternService, IGeneticAlgorithm<StringPatternProcessor> geneticAlgorithm, IRawTextRepository rawTextRepository, ILog log, ITextOptimizationConfig textOptimizationConfig)
        {
            this.stringPatternService = stringPatternService;
            this.geneticAlgorithm = geneticAlgorithm;
            this.rawTextRepository = rawTextRepository;
            this.log = log;
            this.textOptimizationConfig = textOptimizationConfig;
        }

        public void RunFromConfig()
        {
            if (this.textOptimizationConfig.UpdateCachedStringPatterns)
            {
                UpdateCachedStringPatterns();
            }

            if (this.textOptimizationConfig.RunGeneticAlgorithm)
            {
                RunGeneticAlgorithm();
            }
        }

        public void UpdateCachedStringPatterns()
        {
            this.log.Info("Updating Cached String Patterns");
            try
            {
                stringPatternService.UpdateCachedStringPatterns();
            }
            catch (Exception e)
            {
                this.log.Error(e);
                throw;
            }
        }

        public void RunGeneticAlgorithm()
        {
            this.log.Info("Running Genetic Algorithm");
            try
            {
                geneticAlgorithm.RunGeneticAlgorithm();
            }
            catch (Exception e)
            {
                this.log.Error(e);
                throw;
            }
        }

        public void CheckTop100()
        {
            var rawTexts = rawTextRepository.GetAllRawText();

            var top100 = rawTexts.OrderByDescending(x => x.Score).Take(100).ToArray();

            log.Debug(GetStats(top100, x => x.Score));

            var nonAlphaNumericRegex = new Regex("[^a-zA-Z0-9 ]");
            var goodWords = top100.SelectMany(x => nonAlphaNumericRegex.Replace(x.Description, "").Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)).Select(x => x.ToLower().Trim());
            var top100DistinctGoodWords = goodWords.Where(x => !ExcludeGoodWords.Contains(x)).GroupBy(x => x).Select(grp => new { Word = grp.Key, Count = grp.Count() }).Where(x => x.Count > 5).OrderByDescending(x => x.Count).Take(100).ToArray();

            log.Debug(string.Join(", ", top100DistinctGoodWords.Select(x => "\"" + x.Word + "\"")));

            foreach (var word in top100DistinctGoodWords)
            {
                log.InfoFormat("Word: {0}, Count: {1}", word.Word, word.Count);
            }
        }

        private static string GetStats(IEnumerable<RawText> rawTexts, Expression<Func<RawText, int?>> selector)
        {
            var selected = rawTexts.Select(selector.Compile()).ToArray();
            return string.Format("Stats {0}: Min = {1}, Max = {2}, Average = {3}", GetMemberName(selector), selected.Min(), selected.Max(), selected.Average());
        }

        private static string GetDistinct<TField>(IEnumerable<RawText> rawTexts, Expression<Func<RawText, TField>> selector)
        {
            var distinct = rawTexts.Select(selector.Compile()).Distinct().OrderBy(x => x);
            return string.Format("Distinct {0}: {1}", GetMemberName(selector), string.Join(", ", distinct));
        }

        private static string GetMemberName<TField>(Expression<Func<RawText, TField>> selector)
        {
            var member = selector.Body as MemberExpression;
            return member == null ? string.Empty : member.Member.Name;
        }
    }
}
