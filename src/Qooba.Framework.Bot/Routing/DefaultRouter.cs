using Qooba.Framework.Bot.Abstractions;
using Qooba.Framework.Bot.Abstractions.Models;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Qooba.Framework.Bot.Routing
{
    public class DefaultRouter : IRouter
    {
        private readonly IRoutingConfiguration routingConfiguration;

        public DefaultRouter(IRoutingConfiguration routingConfiguration)
        {
            this.routingConfiguration = routingConfiguration;
        }

        public int Priority => 1000;

        public async Task<Route> FindRouteAsync(string text) => this.routingConfiguration.RoutingTable.FirstOrDefault(x => x.IsDefault);

        public Task<IDictionary<string, object>> FindRouteData(string text, IEnumerable<string> routeTexts) => Task.FromResult<IDictionary<string, object>>(null);
    }
}