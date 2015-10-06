namespace Axh.Optimization.Repositories.StringPatterns
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Axh.Optimization.Config.Contracts;
    using Axh.Optimization.DomainModels.StringPatterns;
    using Axh.Optimization.Repositories.StringPatterns.Contracts;

    using MongoDB.Driver;
    using MongoDB.Driver.Builders;

    public class MongoStringPatternProcessorRepository : IStringPatternProcessorRepository
    {
        private readonly MongoCollection<StringPatternProcessorResult> stringPatternProcessorCollection;

        private readonly IStringPatternProcessorFactory stringPatternProcessorFactory;

        public MongoStringPatternProcessorRepository(IOptimizerConfig optimizerConfig, IStringPatternProcessorFactory stringPatternProcessorFactory)
        {
            this.stringPatternProcessorFactory = stringPatternProcessorFactory;
            var client = new MongoClient(optimizerConfig.MongoConnectionString);
            var server = client.GetServer();
            var db = server.GetDatabase(string.Format("{0}-{1}", optimizerConfig.MongoDatabasePrefix, optimizerConfig.InstanceId));

            var collectionName = typeof(StringPatternProcessorResult).Name;

            if (!db.CollectionExists(collectionName))
            {
                var collectionOptions = CollectionOptions.SetCapped(true).SetMaxSize(524288000);
                db.CreateCollection(collectionName, collectionOptions);
            }

            this.stringPatternProcessorCollection = db.GetCollection<StringPatternProcessorResult>(collectionName);
            
            var keys = IndexKeys<StringPatternProcessorResult>.Ascending(x => x.GenerationNumber, x => x.PopulationRank);
            var options = IndexOptions.SetName("GenerationNumber And PopulationRank").SetUnique(true).SetBackground(true);
            this.stringPatternProcessorCollection.CreateIndex(keys, options);
        }

        public void AddStringPatternProcessors(int generationNumber, IEnumerable<StringPatternProcessor> stringPatternProcessors)
        {
            var stringPatternProcessorResults =
                stringPatternProcessors.Select((x, i) => this.stringPatternProcessorFactory.GetStringPatternProcessorResult(x, generationNumber, i)).ToDictionary(x => x.Id);


            // Update cloned candidates
            var existingIds =
                this.stringPatternProcessorCollection.Find(Query<StringPatternProcessorResult>.In(x => x.Id, stringPatternProcessorResults.Values.Select(x => x.Id)))
                    .SetFields(new FieldsBuilder<StringPatternProcessorResult>().Include(x => x.Id))
                    .Select(x => x.Id);
            foreach (var id in existingIds)
            {
                var newCandidate = stringPatternProcessorResults[id];
                var update = new UpdateBuilder<StringPatternProcessorResult>().Set(x => x.GenerationNumber, generationNumber).Set(x => x.PopulationRank, newCandidate.PopulationRank);
                this.stringPatternProcessorCollection.Update(Query<StringPatternProcessorResult>.EQ(x => x.Id, id), update);
                stringPatternProcessorResults.Remove(id);
            }

            // Batch insert the rest
            var results = this.stringPatternProcessorCollection.InsertBatch(stringPatternProcessorResults.Values);

            var badResults = results.Where(x => !x.Ok).ToArray();
            if (badResults.Any())
            {
                var msg = string.Join(", ", badResults.Select(x => x.ErrorMessage));
                throw new Exception(msg);
            }
        }

        public IEnumerable<StringPatternProcessor> GetStringPatternProcessors(int generationNumber)
        {
            return
                this.stringPatternProcessorCollection.Find(Query<StringPatternProcessorResult>.EQ(x => x.GenerationNumber, generationNumber))
                    .Select(this.stringPatternProcessorFactory.GetStringPatternProcessor)
                    .ToList();
        }
    }
}
