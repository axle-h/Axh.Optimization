namespace Axh.TextOptimization.DependencyInjection
{
    using Axh.Optimization.Config;
    using Axh.Optimization.Config.Contracts;
    using Axh.Optimization.DomainModels.StringPatterns;
    using Axh.Optimization.GeneticAlgorithm;
    using Axh.Optimization.GeneticAlgorithm.Contracts;
    using Axh.Optimization.GeneticAlgorithm.Contracts.Extensibility;
    using Axh.Optimization.GeneticAlgorithm.StringPattern;
    using Axh.Optimization.Repositories.StringPatterns;
    using Axh.Optimization.Repositories.StringPatterns.Contracts;
    using Axh.Optimization.Repositories.StringPatterns.Factories;
    using Axh.Optimization.Services.RandomService;
    using Axh.Optimization.Services.RandomService.Contracts;
    using Axh.Optimization.Services.SpellCheckerService;
    using Axh.Optimization.Services.SpellCheckerService.Contracts;
    using Axh.TextOptimization.GeneticAlgorithm;

    using Ninject.Modules;

    public class OptimizerModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind<IOptimizerConfig>().To<OptimizerConfig>();
            this.Bind<IGeneticAlgorithm<StringPatternProcessor>>().To<GeneticAlgorithm<StringPatternProcessor>>();
            this.Bind<IGeneticAlgorithmStateFactory>().To<GeneticAlgorithmStateFactory>();
            this.Bind<IStateLogger<StringPatternProcessor>>().To<StateLogger>();

            this.Bind<IEvolutionStrategy>().To<EvolutionStrategy>();
            this.Bind<IGod<StringPatternProcessor>>().To<StringPatternProcessorGod>();
            this.Bind<IExitStrategy>().To<InfiniteExitStrategy>();

            this.Bind<ISpellCheckerService>().To<ThreadSafeCachedSpellCheckerService>();
            this.Bind<IRandomService>().To<MathDotNetRandomService>();

            this.Bind<IStringPatternRepository>().To<MongoStringPatternRepository>();
            this.Bind<IStringPatternProcessorRepository>().To<MongoStringPatternProcessorRepository>();
            this.Bind<IStringPatternProcessorFactory>().To<StringPatternProcessorFactory>();
        }
    }
}
