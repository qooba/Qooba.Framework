using Newtonsoft.Json;
using Qooba.Framework.Bot.Abstractions;
using Qooba.Framework.Bot.Abstractions.Models;
using Qooba.Framework.Configuration.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Qooba.Framework.Bot
{
    public class ReplyManager : IReplyManager
    {
        private static string botConfigurationPath;

        private static Lazy<ReplyConfiguration> configuration = new Lazy<ReplyConfiguration>(() =>
        {
            var config = File.ReadAllText(botConfigurationPath);
            var configuration = JsonConvert.DeserializeObject<ReplyConfiguration>(config, Serialization.Settings);
            return configuration;
        });

        private static Lazy<IList<Route>> routeTable = new Lazy<IList<Route>>(() =>
        {
            return configuration.Value.Items.SelectMany(x => x.Routes.Select(r => new Route
            {
                RouteId = x.ReplyId,
                RouteText = r
            })).ToList();
        });

        private Func<string, IReplyBuilder> replyBuilders;

        public ReplyManager(IConfig config, Func<string, IReplyBuilder> replyBuilders)
        {
            botConfigurationPath = config[Constants.BotConfigurationPath];
            this.Configuration = configuration.Value;
            this.replyBuilders = replyBuilders;
        }

        public ReplyConfiguration Configuration { get; private set; }

        public async Task<Reply> CreateAsync(IConversationContext context)
        {
            var replyItem = this.Configuration.Items.FirstOrDefault(x => x.ReplyId == context.Route.RouteId);
            var builder = replyBuilders(replyItem.ReplyType);
            var reply = await builder.BuildAsync(context, replyItem);
            return reply;
        }

        public async Task<IList<Route>> FetchRoutingTableAsync() => routeTable.Value;
    }
}