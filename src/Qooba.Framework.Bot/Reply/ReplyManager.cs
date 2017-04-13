using Qooba.Framework.Bot.Abstractions;
using Qooba.Framework.Bot.Abstractions.Models;
using Qooba.Framework.Configuration.Abstractions;
using Qooba.Framework.Serialization.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Qooba.Framework.Bot
{
    public class ReplyManager : IReplyManager
    {
        private Func<string, IReplyBuilder> replyBuilders;

        private readonly ISerializer serializer;

        public ReplyManager(IBotConfig config, ISerializer serializer, Func<string, IReplyBuilder> replyBuilders)
        {
            var botConfigurationPath = config.BotConfigurationPath;
            var botConfig = File.ReadAllText(botConfigurationPath);
            this.serializer = serializer;

            this.Configuration = this.serializer.Deserialize<ReplyConfiguration>(botConfig);

            this.RouteTable = this.Configuration.Items.SelectMany(x => x.Routes.Select(r => new Route
            {
                RouteId = x.ReplyId,
                RouteText = r
            })).ToList();

            this.replyBuilders = replyBuilders;
        }

        public ReplyConfiguration Configuration { get; private set; }

        public IList<Route> RouteTable { get; private set; }

        public async Task<Reply> CreateAsync(IConversationContext context)
        {
            var replyItem = this.Configuration.Items.FirstOrDefault(x => x.ReplyId == context.Route.RouteId);
            var builder = replyBuilders(replyItem.ReplyType);
            var replyBuilderInput = replyItem.Reply.ToObject(builder.ReplyItemType);
            var message = await builder.BuildAsync(context, replyBuilderInput);

            var reply = new Reply
            {
                Recipient = new Recipient { Id = context?.Entry?.Message?.Sender?.Id },
                NotificationType = replyItem.NotificationType,
                SenderAction = replyItem.SenderAction,
                Message = message
            };

            return reply;
        }

        public async Task<IList<Route>> FetchRoutingTableAsync() => RouteTable;
    }
}