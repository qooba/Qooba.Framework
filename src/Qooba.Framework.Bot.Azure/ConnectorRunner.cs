using Qooba.Framework.Bot.Abstractions;
using System.Net.Http;
using System.Threading.Tasks;

namespace Qooba.Framework.Bot.Azure
{
    public class ConnectorRunner : BaseRunner
    {
        public static async Task<HttpResponseMessage> Run(HttpRequestMessage req)
        {
            var serviceProvider = ServiceProvider;
            var connectorType = serviceProvider.GetService<IBotConfig>().BotConnectorType;
            return await serviceProvider.GetService<IConnector>(connectorType).Process(req);
        }
    }
}