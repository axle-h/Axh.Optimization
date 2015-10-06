namespace Axh.TextOptimization.Repositories
{
    using System;
    using System.IO;

    using Axh.Optimization.Config.Contracts;
    using Axh.Optimization.GeneticAlgorithm.Contracts.Models;
    using Axh.Optimization.GeneticAlgorithm.StringPattern.Contracts.Repositories;

    using CsvHelper;
    using CsvHelper.Configuration;

    public class CsvGenerationStateRepository : IGenerationStateRepository, IDisposable
    {
        private readonly IOptimizerConfig optimizerConfig;

        private ICsvWriter csvWriter;

        private TextWriter textWriter;

        private bool csvFileOpen;

        public CsvGenerationStateRepository(IOptimizerConfig optimizerConfig)
        {
            this.csvFileOpen = false;
            this.optimizerConfig = optimizerConfig;
        }

        private void OpenCsvFile()
        {
            var fileName = string.Format("{0}-Generations.csv", optimizerConfig.InstanceId);
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            this.textWriter = File.CreateText(fileName);
            this.csvWriter = new CsvWriter(textWriter);
            csvWriter.Configuration.RegisterClassMap<GenerationStateMap>();
            this.csvFileOpen = true;
        }

        public void AddGenerationState(GenerationState generationState)
        {
            if (!csvFileOpen)
            {
                OpenCsvFile();
            }

            this.csvWriter.WriteRecord(generationState);
            this.textWriter.Flush();
        }

        public void Dispose()
        {
            // No need to dispose the TextWriter instance as CsvHelper does it for us.
            // This is safe to call repeatedly.
            this.csvWriter.Dispose();
        }


        // ReSharper disable once ClassNeverInstantiated.Local
        sealed class GenerationStateMap : CsvClassMap<GenerationState>
        {
            public GenerationStateMap()
            {
                this.Map(x => x.GenerationNumber).Name("GenerationNumber");
                this.Map(x => x.MaxScore).Name("MaxScore");
                this.Map(x => x.MinScore).Name("MinScore");
                this.Map(x => x.AverageScore).Name("AverageScore");
                this.Map(x => x.TopDecileAverageScore).Name("TopDecileAverageScore");
            }
        }
    }
}
