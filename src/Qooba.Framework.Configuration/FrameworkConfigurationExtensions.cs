using System;
using Microsoft.Extensions.Configuration;
using Qooba.Framework.Abstractions;
using Qooba.Framework.Configuration.Abstractions;

namespace Qooba.Framework.Configuration
{
    public static class FrameworkConfigurationExtensions
    {
        private static ConfigurationBuilder builder = new ConfigurationBuilder();

        private static IConfig config;

        public static IFramework AddConfigurationRoot(this IFramework framework, IConfigurationRoot configurationRoot)
        {
            config = new Config(configurationRoot);
            framework.AddSingletonService(typeof(IConfig), config);
            return framework;
        }

        public static IFramework AddConfiguration(this IFramework framework, Func<IConfigurationBuilder, IConfigurationBuilder> configuration)
        {
            configuration(builder);
            return framework;
        }

        public static IFramework AddConfigurationJsonFile(this IFramework framework, string path)
        {
            builder.AddJsonFile(path);
            return framework;
        }

        public static IFramework BuildConfiguration(this IFramework framework)
        {
            config = new Config(builder.Build());
            framework.AddSingletonService(typeof(IConfig), config);
            return framework;
        }

        public static IFramework AddSingletonService(this IFramework framework, Type serviceType, Func<IConfig, object> implementationInstanceFunc)
        {
            framework.AddSingletonService(serviceType, implementationInstanceFunc(config));
            return framework;
        }
    }
}
