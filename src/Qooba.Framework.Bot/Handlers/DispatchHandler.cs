using Qooba.Framework.Bot.Abstractions;
using System.Threading.Tasks;

namespace Qooba.Framework.Bot
{
    public class DispatchHandler : BaseHandler, IHandler
    {
        private readonly IDispatcher replyClient;

        public DispatchHandler(IDispatcher replyClient)
        {
            this.replyClient = replyClient;
        }

        public override int Priority => 3;

        public override async Task InvokeAsync(IConversationContext conversationContext)
        {
            await this.replyClient.SendAsync(conversationContext.Reply);
            await base.InvokeAsync(conversationContext);
        }
    }
}