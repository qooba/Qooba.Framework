using Qooba.Framework.Azure.Storage.Abstractions;
using Qooba.Framework.Configuration.Abstractions;

namespace Qooba.Framework.Azure.Storage
{
    public class AzureStorageConfig : IAzureStorageConfig
    {
        private readonly IConfig config;

        public AzureStorageConfig(IConfig config)
        {
            this.config = config;
        }

        public string StorageConnectionString => this.config["Data:DefaultConnection:StorageConnectionString"];
    }
}
