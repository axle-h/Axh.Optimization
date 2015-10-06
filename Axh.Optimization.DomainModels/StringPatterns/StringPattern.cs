namespace Axh.Optimization.DomainModels.StringPatterns
{
    using System;

    using MongoDB.Bson.Serialization.Attributes;

    public class StringPattern
    {
        [BsonId]
        public Guid Id { get; set; }

        public string Description { get; set; }

        public int KnownScore { get; set; }

        public string[] Words { get; set; }

        public TwoWords[] DoubleWords { get; set; }

        public ThreeWords[] TripleWords { get; set; }

    }
}
