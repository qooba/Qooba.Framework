using Qooba.Framework.Bot.Abstractions;
using Qooba.Framework.Bot.Abstractions.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Qooba.Framework.Bot.Routing
{
    public class Router : IRouter
    {
        private readonly IRoutingConfiguration routingConfiguration;

        public Router(IRoutingConfiguration routingConfiguration)
        {
            this.routingConfiguration = routingConfiguration;
        }

        public async Task<Route> FindRouteAsync(string text)
        {
            return this.routingConfiguration.RoutingTable.FirstOrDefault(x => x.RouteText == text);
        }
    }
}