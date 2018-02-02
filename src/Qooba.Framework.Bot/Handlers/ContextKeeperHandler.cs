using Qooba.Framework.Bot.Abstractions;
using Qooba.Framework.Bot.Abstractions.Models;
using System.Threading.Tasks;
using System.Linq;

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
            if (conversationContext.StateAction == StateAction.Keep)
            {
                await this.stateManager.SaveContextAsync(conversationContext);
            }
            else if (conversationContext.StateAction == StateAction.Clear)
            {
                await this.stateManager.ClearContextAsync(conversationContext);
            }
            else if (conversationContext.StateAction == StateAction.None && conversationContext?.Reply?.Message?.Quick_replies?.Any() == true)
            {
                await this.stateManager.SaveContextAsync(conversationContext);
            }

            await base.InvokeAsync(conversationContext);
        }
    }
}