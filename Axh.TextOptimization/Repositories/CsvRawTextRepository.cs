namespace Axh.TextOptimization.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Linq.Expressions;

    using Axh.TextOptimization.Contracts;
    using Axh.TextOptimization.Contracts.Config;
    using Axh.TextOptimization.Contracts.Repositories;

    using CsvHelper;
    using CsvHelper.Configuration;

    public class CsvRawTextRepository : IRawTextRepository
    {
        private readonly ITextOptimizationConfig textOptimizationConfig;

        public CsvRawTextRepository(ITextOptimizationConfig textOptimizationConfig)
        {
            this.textOptimizationConfig = textOptimizationConfig;
        }

        public IList<RawText> GetAllRawText()
        {
            using (var stream = new StreamReader(this.textOptimizationConfig.CsvFile))
            {
                var csv = new CsvReader(stream);
                csv.Configuration.RegisterClassMap<RawTextMap>();
                var sixMonths = new DateTime(2014, 7, 1);
                var records = csv.GetRecords<RawText>().Where(x => x.DateCreated > sixMonths).ToList();

                RemoveWhere(records, x => !x.PositiveVotes.HasValue);
                
                return records;
            }
        }

        private static void RemoveWhere(ICollection<RawText> rawTexts, Expression<Func<RawText, bool>> where)
        {
            foreach (var rawText in rawTexts.Where(where.Compile()).ToArray())
            {
                rawTexts.Remove(rawText);
            }
        }

        // ReSharper disable once ClassNeverInstantiated.Local
        sealed class RawTextMap : CsvClassMap<RawText>
        {
            public RawTextMap()
            {
                this.Map(x => x.Id).Name("Id");
                this.Map(x => x.DateCreated).Name("DateCreated");
                this.Map(x => x.Rating).Name("Rating");
                this.Map(x => x.Description).Name("Description");
                this.Map(x => x.PositiveVotes).Name("PositiveVotes");
                this.Map(x => x.NegativeVotes).Name("NegativeVotes");
            }
        }
    }
}
