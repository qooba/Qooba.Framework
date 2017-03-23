using Qooba.Framework.Abstractions;
using Qooba.Framework.Azure.Storage;
using Qooba.Framework.Configuration.AppConfiguration;
using Qooba.Framework.DependencyInjection.ContainerFactory;
using Qooba.Framework.DependencyInjection.SimpleContainer;
using Qooba.Framework.Logging.AzureApplicationInsights;
using Qooba.Framework.Serialization;

namespace Qooba.Framework.Bot.Azure
{
    public abstract class BaseRunner
    {
        protected static IContainer Container => Bootstrapper.Instance.BootstrappModules(
                new SimpleContainerModule(),
                new AppConfigModule(),
                new SerializationModule(),
                new AzureStorageModule(),
                new BotModule(),
                new BotAzureModule(),
                new ContainerFactoryModule(),
                new AzureApplicationInsightsModule()).Container;
    }
}