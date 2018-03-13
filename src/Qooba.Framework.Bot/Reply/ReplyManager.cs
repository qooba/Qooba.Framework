using Qooba.Framework.Abstractions;
using Qooba.Framework.Bot.Abstractions;
using Qooba.Framework.Bot.Abstractions.Models;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Qooba.Framework.Bot
{
    public class ReplyManager : IReplyConfiguration, IRoutingConfiguration
    {
        private readonly ISerializer serializer;

        private static ConcurrentBag<ReplyItem> replyItems = new ConcurrentBag<ReplyItem>();

        private static ConcurrentBag<Route> routingTable = new ConcurrentBag<Route>();

        private static Route defaultRoute;

        public ReplyManager(IBotConfig config, ISerializer serializer, IEnumerable<ReplyItem> registeredReplyItems)
        {
            var botConfigurationPath = config.BotConfigurationPath;
            if (!string.IsNullOrEmpty(botConfigurationPath))
            {
                var botConfig = File.ReadAllText(botConfigurationPath);
                this.serializer = serializer;

                var configuration = this.serializer.Deserialize<ReplyConfiguration>(botConfig);
                configuration.Items.ToList().ForEach(x => replyItems.Add(x));
                configuration.Items.SelectMany(x => x.Routes.Select(r => new Route
                {
                    RouteId = x.ReplyId,
                    RouteText = r,
                    IsDefault = x.IsDefault,
                    IsGlobalCommand = x.IsGlobalCommand
                })).ToList().ForEach(x => routingTable.Add(x));
            }

            registeredReplyItems.Where(x => x.ReplyId != null).ToList().ForEach(x => this.AddConfiguration(x));
        }

        public IList<Route> RoutingTable => routingTable.ToList();

        public void AddConfiguration(ReplyItem replyItem)
        {
            if (!replyItems.ToList().Any(x => x.ReplyId == replyItem.ReplyId))
            {
                replyItems.Add(replyItem);

                replyItem.Routes.ToList().ForEach(x =>
                routingTable.Add(new Route
                {
                    RouteId = replyItem.ReplyId,
                    RouteText = x,
                    IsDefault = replyItem.IsDefault,
                    IsGlobalCommand = replyItem.IsGlobalCommand
                }));
            }
        }

        public async Task<ReplyItem> FetchReplyItem(IConversationContext context) => replyItems.FirstOrDefault(x => x.ReplyId == context.Route.RouteId);
    }
}