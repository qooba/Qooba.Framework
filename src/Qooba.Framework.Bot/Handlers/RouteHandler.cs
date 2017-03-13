using Qooba.Framework.Bot.Abstractions;
using System.Threading.Tasks;

namespace Qooba.Framework.Bot.Handlers
{
    public class RouteHandler : BaseHandler, IHandler
    {
        private readonly IRouter router;

        public RouteHandler(IRouter router)
        {
            this.router = router;
        }

        public override int Priority => 1;

        public override async Task InvokeAsync(IConversationContext conversationContext)
        {
            conversationContext.Route = await this.router.FindRouteAsync(conversationContext.Entry.Message.Message.Text);
            await base.InvokeAsync(conversationContext);
        }
    }
}