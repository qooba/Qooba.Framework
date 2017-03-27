using Qooba.Framework.Configuration.Abstractions;
using Qooba.Framework.Azure.IoT.Abstractions;

namespace Qooba.Framework.Azure.IoT
{
    public class IoTHubConfig : IIoTHubConfig
    {
        private IConfig config;

        public IoTHubConfig(IConfig config)
        {
            this.config = config;
        }

        public string ConnectionString => this.config["Data:DefaultConnection:ConnectionString"];
    }
}
