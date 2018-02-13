using Qooba.Framework.Configuration.Abstractions;
using Microsoft.Extensions.Configuration;

namespace Qooba.Framework.Configuration
{
    public class Config : IConfig
    {
        private IConfigurationRoot configuration;

        public Config(IConfigurationRoot configuration)
        {
            this.configuration = configuration;
        }

        public string this[string key] => this.configuration[key];
    }
}