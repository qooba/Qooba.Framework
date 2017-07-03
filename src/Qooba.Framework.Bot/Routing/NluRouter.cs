using Qooba.Framework.Bot.Abstractions;
using Qooba.Framework.Bot.Abstractions.Models;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Qooba.Framework.Bot.Routing
{
    public class NluRouter : IRouter
    {
        private readonly IRoutingConfiguration routingConfiguration;

        private readonly INlu nlu;

        public NluRouter(IRoutingConfiguration routingConfiguration, INlu nlu)
        {
            this.routingConfiguration = routingConfiguration;
            this.nlu = nlu;
        }

        public int Priority => 10;

        public async Task<Route> FindRouteAsync(IConversationContext conversationContext)
        {
            var text = conversationContext?.Entry?.Message?.Message?.Text;
            //TODO: INlu.Process
            return null;
        }

        public Task<IDictionary<string, object>> FindRouteData(string text, IEnumerable<string> routeTexts)
        {
            //TODO:
            return null;
        }
    }
}