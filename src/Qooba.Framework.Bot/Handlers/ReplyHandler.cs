using Qooba.Framework.Bot.Abstractions;
using System;
using System.Threading.Tasks;

namespace Qooba.Framework.Bot.Handlers
{
    public class ReplyHandler : BaseHandler, IHandler
    {
        private readonly Func<string, IReplyBuilder> replyBuilders;

        private readonly IReplyConfiguration replyConfiguration;

        public ReplyHandler(IReplyConfiguration replyConfiguration, Func<string, IReplyBuilder> replyBuilders)
        {
            this.replyConfiguration = replyConfiguration;
            this.replyBuilders = replyBuilders;
        }

        public override int Priority => 2;

        public override async Task InvokeAsync(IConversationContext conversationContext)
        {
            var replyItem = await this.replyConfiguration.FetchReplyItem(conversationContext);
            var builder = replyBuilders(replyItem.ReplyType);
            conversationContext.Reply = await builder.BuildAsync(conversationContext, replyItem);
            await base.InvokeAsync(conversationContext);
        }
    }
}