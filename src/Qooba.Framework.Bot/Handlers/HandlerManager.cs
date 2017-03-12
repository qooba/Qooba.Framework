using Qooba.Framework.Bot.Abstractions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Qooba.Framework.Bot
{
    public class HandlerManager : IHandlerManager
    {
        IEnumerable<IHandler> handlers;

        public HandlerManager(IEnumerable<IHandler> handlers)
        {
            this.handlers = handlers;
        }

        public async Task<IHandler> CreateAsync(IConversationContext conversationContext)
        {
            var handlers = this.handlers.OrderBy(x => x.Priority).ToList();
            IHandler firstHandler = null;
            IHandler currentHandler = null;
            foreach (var handler in handlers)
            {
                if (firstHandler == null)
                {
                    firstHandler = handler;
                }
                else
                {
                    currentHandler.NextHandler = handler;
                }

                currentHandler = handler;
            }

            return firstHandler;
        }
    }
}