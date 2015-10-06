namespace Axh.Optimization.DomainModels.StringPatterns
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class StringPatternProcessor
    {
        private readonly Guid id;

        public StringPatternProcessor(Guid id, IDictionary<string, double> singleWordPatterns, IDictionary<TwoWords, double> doubleWordPatterns, IDictionary<ThreeWords, double> tripleWordPatterns)
        {
            this.id = id;
            this.DoubleWordPatterns = doubleWordPatterns;
            this.SingleWordPatterns = singleWordPatterns;
            this.TripleWordPatterns = tripleWordPatterns;
        }

        public Guid Id
        {
            get
            {
                return id;
            }
        }

        public IDictionary<string, double> SingleWordPatterns { get; private set; }

        public IDictionary<TwoWords, double> DoubleWordPatterns { get; private set; }

        public IDictionary<ThreeWords, double> TripleWordPatterns { get; private set; }

        public int ProcessPatterns(ICollection<string> singleWords, ICollection<TwoWords> doubleWords, ICollection<ThreeWords> tripleWords)
        {
            var score = singleWords.Where(word => this.SingleWordPatterns.ContainsKey(word)).Sum(word => this.SingleWordPatterns[word])
                        + doubleWords.Where(word => this.DoubleWordPatterns.ContainsKey(word)).Sum(word => this.DoubleWordPatterns[word])
                        + tripleWords.Where(word => this.TripleWordPatterns.ContainsKey(word)).Sum(word => this.TripleWordPatterns[word]);
            
            return (int)Math.Round(score);
        }

        public override string ToString()
        {
            return string.Format("{0} | {1} | {2}",
                string.Join(", ", this.SingleWordPatterns.Select(x => string.Format("{0} ({1})", x.Key, x.Value))),
                string.Join(", ", this.DoubleWordPatterns.Select(x => string.Format("{0} {1} ({2})", x.Key.Word1, x.Key.Word2, x.Value))),
                string.Join(", ", this.TripleWordPatterns.Select(x => string.Format("{0} {1} {2} ({3})", x.Key.Word1, x.Key.Word2, x.Key.Word3, x.Value))));
        }
    }
}
