using Qooba.Framework.Bot.Abstractions;
using System.Threading.Tasks;

namespace Qooba.Framework.Bot.Handlers
{
    public class ReplyHandler : BaseHandler, IHandler
    {
        private readonly IReplyManager replyManager;

        public ReplyHandler(IReplyManager replyManager)
        {
            this.replyManager = replyManager;
        }

        public override int Priority => 2;

        public override async Task InvokeAsync(IConversationContext conversationContext)
        {
            conversationContext.Reply = await this.replyManager.CreateAsync(conversationContext);
            await base.InvokeAsync(conversationContext);
        }
    }
}