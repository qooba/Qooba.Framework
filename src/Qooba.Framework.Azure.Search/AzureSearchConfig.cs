using Qooba.Framework.Azure.Search.Abstractions;
using Qooba.Framework.Configuration.Abstractions;

namespace Qooba.Framework.Azure.Search
{
    public class AzureSearchConfig : IAzureSearchConfig
    {
        private readonly IConfig config;

        public AzureSearchConfig(IConfig config)
        {
            this.config = config;
        }

        public string SearchApiKey => this.config["Data:DefaultConnection:SearchApiKey"];

        public string SearchServiceName => this.config["Data:DefaultConnection:SearchServiceName"];
    }
}
