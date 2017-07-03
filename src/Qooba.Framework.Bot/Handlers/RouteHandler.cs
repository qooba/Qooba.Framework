using Qooba.Framework.Bot.Abstractions;
using Qooba.Framework.Bot.Abstractions.Models;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Qooba.Framework.Bot.Handlers
{
    public class RouteHandler : BaseHandler, IHandler
    {
        private readonly IEnumerable<IRouter> routers;

        public RouteHandler(IEnumerable<IRouter> routers)
        {
            this.routers = routers.OrderBy(x => x.Priority);
        }

        public override int Priority => 2;

        public override async Task InvokeAsync(IConversationContext conversationContext)
        {
            if (conversationContext.Route == null)
            {
                foreach (var router in this.routers)
                {
                    conversationContext.Route = await router.FindRouteAsync(conversationContext);
                    if (conversationContext.Route != null)
                    {
                        break;
                    }
                }
            }

            await base.InvokeAsync(conversationContext);
        }
    }
}