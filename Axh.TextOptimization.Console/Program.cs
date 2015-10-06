namespace Axh.TextOptimization.Console
{
    using Axh.TextOptimization.DependencyInjection;

    using log4net.Config;

    class Program
    {
        static void Main(string[] args)
        {
            XmlConfigurator.Configure();
            var bootStrapper = new BootStrapper();
            var application = bootStrapper.GetApplciation();
            application.RunFromConfig();
        }
    }
}
