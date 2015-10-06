namespace Axh.TextOptimization.Contracts
{
    using System;

    [Serializable]
    public class RawText
    {
        public Guid Id { get; set; }
        public DateTime DateCreated { get; set; }
        public int? Rating { get; set; }
        public string Description { get; set; }
        public int? PositiveVotes { get; set; }
        public int? NegativeVotes { get; set; }

        public int Score
        {
            get
            {
                var votes = (this.PositiveVotes - this.NegativeVotes).GetValueOrDefault();
                return this.Rating.GetValueOrDefault() * 20 + Math.Max(Math.Min(votes, 20), -20);
            }
        }

        public override string ToString()
        {
            return $"Id: {Id}, DateCreated: {DateCreated}, Rating: {Rating}, Description: {Description}, PositiveVotes: {PositiveVotes}, NegativeVotes: {NegativeVotes}, Score: {Score}";
        }
    }
}
