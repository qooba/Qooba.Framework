using Qooba.Framework.Bot.Abstractions;
using System.Threading.Tasks;

namespace Qooba.Framework.Bot.Handlers
{
    public class ReplyHandler : BaseHandler, IHandler
    {
        private readonly IReplyFactory replyFactory;

        private readonly IReplyConfiguration replyConfiguration;

        public ReplyHandler(IReplyConfiguration replyConfiguration, IReplyFactory replyFactory)
        {
            this.replyFactory = replyFactory;
            this.replyConfiguration = replyConfiguration;
        }

        public override int Priority => 3;

        public override async Task InvokeAsync(IConversationContext conversationContext)
        {
            var replyItem = await this.replyConfiguration.FetchReplyItem(conversationContext);
            conversationContext.Reply = await this.replyFactory.CreateReplyAsync(conversationContext, replyItem);
            await base.InvokeAsync(conversationContext);
        }
    }
}