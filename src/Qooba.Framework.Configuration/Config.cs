using Qooba.Framework.Configuration.Abstractions;
#if NET46
using System.Configuration;
#else
using Microsoft.Extensions.Configuration;
#endif

namespace Qooba.Framework.Configuration
{
    public class Config : IConfig
    {
#if NET46
        public string this[string key] => ConfigurationManager.AppSettings[key];
#else
        private IConfigurationRoot configuration;

        public Config(IConfigurationRoot configuration)
        {
            this.configuration = configuration;
        }

        public string this[string key] => this.configuration[key];
#endif

    }
}