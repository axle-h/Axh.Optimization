namespace Axh.TextOptimization.DependencyInjection
{
    using Axh.TextOptimization.Contracts;

    using Ninject;

    public class BootStrapper
    {
        private readonly IKernel kernel;

        public BootStrapper()
        {
            this.kernel = new StandardKernel();
            this.kernel.Load(new ApplicationModule(), new OptimizerModule());
        }

        public T Resolve<T>()
        {
            return this.kernel.Get<T>();
        }

        public ITextOptimization GetApplciation()
        {
            return this.kernel.Get<ITextOptimization>();
        }
    }
}
