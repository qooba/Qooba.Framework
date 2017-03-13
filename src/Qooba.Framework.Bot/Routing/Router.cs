using Qooba.Framework.Bot.Abstractions;
using Qooba.Framework.Bot.Abstractions.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Qooba.Framework.Bot.Routing
{
    public class Router : IRouter
    {
        private readonly IReplyManager replyManager;

        public Router(IReplyManager replyManager)
        {
            this.replyManager = replyManager;
        }

        public async Task<Route> FindRouteAsync(string text)
        {
            var routeTable = await this.replyManager.FetchRoutingTableAsync();
            return routeTable.FirstOrDefault(x => x.RouteText == text);
        }
    }
}