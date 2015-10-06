namespace Axh.Optimization.DomainModels.StringPatterns
{
    using MongoDB.Bson.Serialization.Attributes;

    public class ThreeWords
    {
        public ThreeWords(string word1, string word2, string word3)
        {
            this.Word1 = word1;
            this.Word2 = word2;
            this.Word3 = word3;
        }

        [BsonElement("W1")]
        public string Word1 { get; set; }

        [BsonElement("W2")]
        public string Word2 { get; set; }

        [BsonElement("W3")]
        public string Word3 { get; set; }

        protected bool Equals(ThreeWords other)
        {
            return string.Equals(this.Word1, other.Word1) && string.Equals(this.Word2, other.Word2) && string.Equals(this.Word3, other.Word3);
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
            return this.Equals((ThreeWords)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (this.Word1 != null ? this.Word1.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.Word2 != null ? this.Word2.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.Word3 != null ? this.Word3.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}
