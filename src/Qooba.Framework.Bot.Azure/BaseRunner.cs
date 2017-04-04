using Qooba.Framework.Abstractions;
using Qooba.Framework.Azure.Storage;
using Qooba.Framework.Configuration;
using Qooba.Framework.DependencyInjection.ContainerFactory;
using Qooba.Framework.DependencyInjection.SimpleContainer;
using Qooba.Framework.Logging.AzureApplicationInsights;
using Qooba.Framework.Serialization;

namespace Qooba.Framework.Bot.Azure
{
    public abstract class BaseRunner
    {
        protected static IServiceProvider ServiceProvider => Q.Create()
            .AddModule(m => m.Module(new SimpleContainerModule()))
            .AddModule(m => m.Module(new ConfigModule()))
            .AddModule(m => m.Module(new SerializationModule()))
            .AddModule(m => m.Module(new AzureStorageModule()))
            .AddModule(m => m.Module(new BotModule()))
            .AddModule(m => m.Module(new BotAzureModule()))
            .AddModule(m => m.Module(new ContainerFactoryModule()))
            .AddModule(m => m.Module(new AzureApplicationInsightsModule()))
            .Bootstrapp();
    }
}