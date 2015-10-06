namespace Axh.TextOptimization.Factories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    using Axh.Optimization.DomainModels.StringPatterns;
    using Axh.Optimization.Services.SpellCheckerService.Contracts;
    using Axh.TextOptimization.Contracts;
    using Axh.TextOptimization.Contracts.Factories;

    public class StringPatternFactory : IStringPatternFactory
    {
        private readonly ISpellCheckerService spellCheckerService;

        public StringPatternFactory(ISpellCheckerService spellCheckerService)
        {
            this.spellCheckerService = spellCheckerService;
        }

        public StringPattern GetStringPattern(RawText rawText)
        {
            var anchorRegex = new Regex(@"<a[^>]*>([^<]+)<\/a>");
            var nonAlphaNumericRegex = new Regex("[^a-zA-Z ]");

            var description = new[] { "<br />", "<br/>", "<br>", "&nbsp;" }.Aggregate(rawText.Description, (current, badWord) => current.Replace(badWord, " "));
            description = anchorRegex.Replace(description, " $1 ");
            description = nonAlphaNumericRegex.Replace(description, "");

            var descriptionWords = description.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.ToLower().Trim()).ToArray();

            descriptionWords = descriptionWords.Distinct().SelectMany(x => this.spellCheckerService.Spell(x).Suggestions).Distinct().ToArray();

            var doubleDescriptionWords = new List<TwoWords>();
            var tripleDescriptionWords = new List<ThreeWords>();

            for (var i = 0; i < descriptionWords.Length; i++)
            {
                var word1 = descriptionWords[i];
                var word2 = i + 1 < descriptionWords.Length ? descriptionWords[i + 1] : null;
                var word3 = i + 2 < descriptionWords.Length ? descriptionWords[i + 2] : null;

                if (word2 != null)
                {
                    doubleDescriptionWords.Add(new TwoWords(word1, word2));
                }

                if (word3 != null)
                {
                    tripleDescriptionWords.Add(new ThreeWords(word1, word2, word3));
                }
            }

            return new StringPattern
            {
                Id = rawText.Id,
                Description = rawText.Description,
                Words = descriptionWords,
                DoubleWords = doubleDescriptionWords.Distinct().ToArray(),
                TripleWords = tripleDescriptionWords.Distinct().ToArray(),
                KnownScore = rawText.Score
            };
        }
    }
}
