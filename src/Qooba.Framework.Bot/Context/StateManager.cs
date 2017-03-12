using Qooba.Framework.Bot.Abstractions;
using Qooba.Framework.Configuration.Abstractions;
using System.Threading.Tasks;

namespace Qooba.Framework.Bot
{
    public class StateManager : IStateManager
    {
        private readonly IConfig config;

        public StateManager(IConfig config)
        {
            this.config = config;
        }

        public async Task<IConversationContext> FetchContext(IConversationContext context)
        {
            return context;
        }

        public async Task SaveContext(IConversationContext context)
        {

        }
    }
}