namespace Axh.Optimization.Services.SpellCheckerService.Contracts
{
    using System.Collections.Generic;

    public class SpellCheckResponse
    {
        public SpellCheckResponse(bool isSpeltCorrectly, IEnumerable<string> suggestions)
        {
            this.IsSpeltCorrectly = isSpeltCorrectly;
            this.Suggestions = suggestions;
        }

        public bool IsSpeltCorrectly { get; set; }

        public IEnumerable<string> Suggestions { get; set; }
    }
}
