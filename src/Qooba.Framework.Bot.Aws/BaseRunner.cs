using Qooba.Framework.Configuration;
using Qooba.Framework.DependencyInjection.SimpleContainer;
using Qooba.Framework.Logging.Console;
using Qooba.Framework.Serialization;
using System;

namespace Qooba.Framework.Bot.Aws
{
    public abstract class BaseRunner
    {
        protected static IServiceProvider ServiceProvider => FrameworkBuilder.Create()
            .AddModule(m => m.Module(new SimpleContainerModule()))
            .AddModule(m => m.Module(new ConfigModule()))
            .AddModule(m => m.Module(new SerializationModule()))
            .AddModule(m => m.Module(new BotModule()))
            .AddModule(m => m.Module(new BotAwsModule()))
            .AddModule(m => m.Module(new ConsoleModule()))
            .Bootstrapp();
    }
}