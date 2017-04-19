using Qooba.Framework.Bot.Abstractions;
using System.Threading.Tasks;

namespace Qooba.Framework.Bot.Handlers
{
    public class ContextKeeperHandler : BaseHandler, IHandler
    {
        private readonly IStateManager stateManager;

        public ContextKeeperHandler(IStateManager stateManager)
        {
            this.stateManager = stateManager;
        }

        public override int Priority => 5;

        public override async Task InvokeAsync(IConversationContext conversationContext)
        {
            if (conversationContext.KeepState)
            {
                await this.stateManager.SaveContextAsync(conversationContext);
            }

            await base.InvokeAsync(conversationContext);
        }
    }
}