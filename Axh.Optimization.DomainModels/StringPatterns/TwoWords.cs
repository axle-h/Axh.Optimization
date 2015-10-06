namespace Axh.Optimization.DomainModels.StringPatterns
{
    using MongoDB.Bson.Serialization.Attributes;

    public class TwoWords
    {
        public TwoWords(string word1, string word2)
        {
            this.Word1 = word1;
            this.Word2 = word2;
        }

        [BsonElement("W1")]
        public string Word1 { get; set; }

        [BsonElement("W2")]
        public string Word2 { get; set; }

        protected bool Equals(TwoWords other)
        {
            return string.Equals(this.Word1, other.Word1) && string.Equals(this.Word2, other.Word2);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            if (obj.GetType() != this.GetType())
            {
                return false;
            }
            return this.Equals((TwoWords)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((this.Word1 != null ? this.Word1.GetHashCode() : 0) * 397) ^ (this.Word2 != null ? this.Word2.GetHashCode() : 0);
            }
        }
    }
}
