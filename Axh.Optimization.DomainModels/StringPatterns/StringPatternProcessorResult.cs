namespace Axh.Optimization.DomainModels.StringPatterns
{
    using System;
    using System.Collections.Generic;

    using MongoDB.Bson.Serialization.Attributes;

    public class StringPatternProcessorResult
    {
        [BsonId]
        public Guid Id { get; set; }

        [BsonElement("nG")]
        public int GenerationNumber { get; set; }

        [BsonElement("nP")]
        public int PopulationRank { get; set; }
        
        [BsonElement("S")]
        public IDictionary<string, double> SingleWordPatterns { get; set; }

        [BsonElement("D")]
        public IDictionary<TwoWords, double> DoubleWordPatterns { get; set; }

        [BsonElement("T")]
        public IDictionary<ThreeWords, double> TripleWordPatterns { get; set; }
    }
}
