namespace Axh.Optimization.Services.SpellCheckerService
{
    using System;
    using System.Collections.Concurrent;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;

    using Axh.Optimization.Services.SpellCheckerService.Contracts;

    using NHunspell;

    public class ThreadSafeCachedSpellCheckerService : ISpellCheckerService
    {
        private readonly SpellEngine spellEngine;

        private bool disposing;

        private readonly ConcurrentDictionary<string, SpellCheckResponse> spellCache;

        private readonly Regex nonAlphaNumericRegex;

        private const string LanguageCode = "en";
        private const string HunspellAffFile = "en_GB.aff";
        private const string HunspellDictFile = "en_GB.dic";
        private const string HunspellDictionariesFolder = "Dictionaries";

        public ThreadSafeCachedSpellCheckerService()
        {
            this.disposing = false;

            this.spellEngine = new SpellEngine();

            var languageConfig = new LanguageConfig
                                 {
                                     LanguageCode = LanguageCode,
                                     HunspellAffFile = Path.Combine(HunspellDictionariesFolder, HunspellAffFile),
                                     HunspellDictFile = Path.Combine(HunspellDictionariesFolder, HunspellDictFile)
                                 };
            this.spellEngine.AddLanguage(languageConfig);

            this.spellCache = new ConcurrentDictionary<string, SpellCheckResponse>();
            this.nonAlphaNumericRegex = new Regex("[^a-zA-Z ]");
        }

        public SpellCheckResponse Spell(string word)
        {
            return this.spellCache.GetOrAdd(word,
                newWord =>
                {
                    var factory = this.spellEngine[LanguageCode];
                    var spell = factory.Spell(newWord);
                    
                    if (spell)
                    {
                        return new SpellCheckResponse(true, new[] { word });
                    }

                    var suggestions = factory.Suggest(newWord);
                    var bestSuggestion = suggestions.FirstOrDefault(x => !this.nonAlphaNumericRegex.IsMatch(x)) ?? word;
                    // Clean suggestion
                    var bestSuggestions = bestSuggestion.ToLower().Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Distinct();

                    return new SpellCheckResponse(false, bestSuggestions);
                });
        }
        
        public void Dispose()
        {
            if (this.disposing || this.spellEngine == null)
            {
                return;
            }

            this.disposing = true;
            this.spellEngine.Dispose();
        }
    }
}
