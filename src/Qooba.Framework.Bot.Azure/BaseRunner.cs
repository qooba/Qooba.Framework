using Qooba.Framework.Abstractions;
using Qooba.Framework.Configuration;
using Qooba.Framework.DependencyInjection.SimpleContainer;
using Qooba.Framework.Logging.AzureApplicationInsights;
using Qooba.Framework.Serialization;

namespace Qooba.Framework.Bot.Azure
{
    public abstract class BaseRunner
    {
        protected static IServiceProvider ServiceProvider => FrameworkBuilder.Create()
            .AddModule(m => m.Module(new SimpleContainerModule()))
            .AddModule(m => m.Module(new ConfigModule()))
            .AddModule(m => m.Module(new SerializationModule()))
            .AddModule(m => m.Module(new BotModule()))
            .AddModule(m => m.Module(new BotAzureModule()))
            .AddModule(m => m.Module(new AzureApplicationInsightsModule()))
            .Bootstrapp();
    }
}