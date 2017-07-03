using Qooba.Framework.Bot.Abstractions;
using Qooba.Framework.Bot.Abstractions.Models;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Qooba.Framework.Bot.Routing
{
    public class AsrRouter : IRouter
    {
        private readonly IRoutingConfiguration routingConfiguration;

        private readonly IAsr asr;

        public AsrRouter(IRoutingConfiguration routingConfiguration, IAsr asr)
        {
            this.routingConfiguration = routingConfiguration;
            this.asr = asr;
        }

        public int Priority => 11;

        public async Task<Route> FindRouteAsync(IConversationContext conversationContext)
        {
            var audio = conversationContext?.Entry?.Message?.Message?.Attachments?.FirstOrDefault(x=>x.Type == Abstractions.Models.Attachments.AttachmentType.audio);
            //TODO: IAsr.Process
            return null;
        }

        public Task<IDictionary<string, object>> FindRouteData(string text, IEnumerable<string> routeTexts)
        {
            //TODO:
            return null;
        }
    }
}