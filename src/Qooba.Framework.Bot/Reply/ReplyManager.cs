using Qooba.Framework.Bot.Abstractions;
using Qooba.Framework.Bot.Abstractions.Models;
using Qooba.Framework.Serialization.Abstractions;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Qooba.Framework.Bot
{
    public class ReplyManager : IReplyConfiguration, IRoutingConfiguration
    {
        private readonly ISerializer serializer;

        private static ReplyConfiguration configuration;

        private static IList<Route> routingTable;

        private static Route defaultRoute;

        public ReplyManager(IBotConfig config, ISerializer serializer)
        {
            if (configuration == null)
            {
                var botConfigurationPath = config.BotConfigurationPath;
                var botConfig = File.ReadAllText(botConfigurationPath);
                this.serializer = serializer;

                configuration = this.serializer.Deserialize<ReplyConfiguration>(botConfig);
                routingTable = configuration.Items.SelectMany(x => x.Routes.Select(r => new Route
                {
                    RouteId = x.ReplyId,
                    RouteText = r,
                    IsDefault = x.IsDefault
                })).ToList();
            }
        }
        
        public IList<Route> RoutingTable => routingTable;
        
        public async Task<ReplyItem> FetchReplyItem(IConversationContext context)
        {
            return configuration.Items.FirstOrDefault(x => x.ReplyId == context.Route.RouteId);
        }
    }
}