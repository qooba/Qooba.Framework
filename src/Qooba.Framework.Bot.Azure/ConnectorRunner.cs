using Microsoft.Azure.WebJobs.Host;
using Qooba.Framework.Bot.Abstractions;
using Qooba.Framework.Configuration.Abstractions;
using System.Net.Http;
using System.Threading.Tasks;

namespace Qooba.Framework.Bot.Azure
{
    public class ConnectorRunner : BaseRunner
    {
        public static async Task<HttpResponseMessage> Run(HttpRequestMessage req, TraceWriter log)
        {
            var connectorType = Container.Resolve<IConfig>()["ConnectorType"];
            return await Container.Resolve<IConnector>(connectorType).Process(req);
        }
    }
}