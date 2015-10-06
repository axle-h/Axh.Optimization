namespace Axh.Optimization.Repositories.StringPatterns
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Axh.Optimization.Config.Contracts;
    using Axh.Optimization.DomainModels.StringPatterns;
    using Axh.Optimization.Repositories.StringPatterns.Contracts;

    using MongoDB.Driver;

    public class MongoStringPatternRepository : IStringPatternRepository
    {
        private readonly MongoCollection<StringPattern> stringPatternCollection;

        public MongoStringPatternRepository(IOptimizerConfig optimizerConfig)
        {
            var client = new MongoClient(optimizerConfig.MongoConnectionString);
            var server = client.GetServer();
            var database = server.GetDatabase(string.Format("{0}-{1}", optimizerConfig.MongoDatabasePrefix, optimizerConfig.InstanceId));
            this.stringPatternCollection = database.GetCollection<StringPattern>(typeof(StringPattern).Name);
        }

        public IEnumerable<StringPattern> GetAllStringPatterns()
        {
            return this.stringPatternCollection.FindAll().ToList();
        }

        public void CacheAllStringPatterns(IEnumerable<StringPattern> patterns)
        {
            var results = this.stringPatternCollection.InsertBatch(patterns);

            var badResults = results.Where(x => !x.Ok).ToArray();
            if (badResults.Any())
            {
                var msg = string.Join(", ", badResults.Select(x => x.ErrorMessage));
                throw new Exception(msg);
            }
        }
    }
}
