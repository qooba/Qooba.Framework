using Qooba.Framework.Bot.Abstractions;
using System.Threading.Tasks;

namespace Qooba.Framework.Bot
{
    public class ContextHandler : BaseHandler, IHandler
    {
        private readonly IStateManager stateManager;

        public ContextHandler(IStateManager stateManager)
        {
            this.stateManager = stateManager;
        }

        public override int Priority => 0;

        public override async Task InvokeAsync(IConversationContext conversationContext)
        {
            conversationContext = await this.stateManager.FetchContext(conversationContext);
            await base.InvokeAsync(conversationContext);
        }
    }
}