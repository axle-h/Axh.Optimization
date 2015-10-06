namespace Axh.TextOptimization.DependencyInjection
{
    using Axh.Optimization.GeneticAlgorithm.StringPattern.Contracts.Repositories;
    using Axh.Optimization.GeneticAlgorithm.StringPattern.Contracts.Services;
    using Axh.TextOptimization.Config;
    using Axh.TextOptimization.Contracts;
    using Axh.TextOptimization.Contracts.Config;
    using Axh.TextOptimization.Contracts.Factories;
    using Axh.TextOptimization.Contracts.Repositories;
    using Axh.TextOptimization.Factories;
    using Axh.TextOptimization.Repositories;
    using Axh.TextOptimization.Services;

    using log4net;

    using Ninject.Modules;

    public class ApplicationModule : NinjectModule
    {
        private const string LoggerName = "TextOptimization";

        public override void Load()
        {
            this.Bind<ITextOptimization>().To<TextOptimization>();
            this.Bind<ILog>().ToConstant(LogManager.GetLogger(LoggerName));
            this.Bind<IStringPatternFactory>().To<StringPatternFactory>();
            this.Bind<IStringPatternService>().To<StringPatternService>();
            this.Bind<IRawTextRepository>().To<CsvRawTextRepository>();
            this.Bind<ITextOptimizationConfig>().To<TextOptimizationConfig>();
            this.Bind<IGenerationStateRepository>().To<CsvGenerationStateRepository>();   
        }
    }
}
