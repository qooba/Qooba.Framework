using Qooba.Framework.Abstractions;
using Microsoft.Extensions.Configuration;

namespace Qooba.Framework.Configuration
{
    public class Config : Abstractions.IConfiguration
    {
        private IConfigurationRoot configuration;

        public Config(IConfigurationRoot configuration)
        {
            this.configuration = configuration;
        }

        public string this[string key] => this.configuration[key];
    }
}