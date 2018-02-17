using System;
using Microsoft.Extensions.Configuration;
using Qooba.Framework.Abstractions;

namespace Qooba.Framework.Configuration
{
    public static class FrameworkConfigurationExtensions
    {
        private static ConfigurationBuilder builder = new ConfigurationBuilder();

        private static Lazy<Abstractions.IConfiguration> config = new Lazy<Abstractions.IConfiguration>(() => new Config(builder.Build()));

        public static IFramework AddConfigurationRoot(this IFramework framework, IConfigurationRoot configurationRoot)
        {
            return framework.AddSingletonService(typeof(Abstractions.IConfiguration), new Config(configurationRoot));   
        }

        public static IFramework AddConfiguration(this IFramework framework, Func<IConfigurationBuilder, IConfigurationBuilder> configuration)
        {
            var configurationRoot = configuration(builder).Build();
            return framework.AddConfigurationRoot(configurationRoot);
        }

        public static IFramework AddConfigurationJsonFile(this IFramework framework, string path)
        {
            builder.AddJsonFile(path);
            return framework.AddSingletonService(typeof(Abstractions.IConfiguration), s => config.Value);   
        }
    }
}