using Qooba.Framework.Bot.Abstractions;
using Qooba.Framework.Bot.Abstractions.Models;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Qooba.Framework.Serialization.Abstractions;
using System.Text;

namespace Qooba.Framework.Bot.Handlers
{
    public class RouteHandler : BaseHandler, IHandler
    {
        private readonly IEnumerable<IRouter> routers;

        private readonly ISerializer serializer;

        public RouteHandler(IEnumerable<IRouter> routers, ISerializer serializer)
        {
            this.routers = routers.OrderBy(x => x.Priority);
            this.serializer = serializer;
        }

        public override int Priority => 2;

        public static string RemoveAccents(string text) => Encoding.UTF8.GetString(Encoding.GetEncoding("ISO-8859-8").GetBytes(text.ToLowerInvariant()));

        public override async Task InvokeAsync(IConversationContext conversationContext)
        {
            if (conversationContext?.Reply?.Message?.Quick_replies?.Any() == true && conversationContext?.Entry?.Message?.Message?.Quick_reply == null)
            {
                var quickReply = conversationContext.Reply.Message.Quick_replies.FirstOrDefault(x => RemoveAccents(x.Title) == RemoveAccents(conversationContext.Entry.Message.Message.Text));
                if (quickReply != null)
                {
                    conversationContext.Entry.Message.Message.Quick_reply = quickReply;
                }
            }

            var payloadRoute = PreparePayloadRoute(conversationContext);

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

            MergePayloadRoute(conversationContext, payloadRoute);

            await base.InvokeAsync(conversationContext);
        }

        private static void MergePayloadRoute(IConversationContext conversationContext, Route payloadRoute)
        {
            if (payloadRoute?.RouteData?.Any() == true)
            {
                conversationContext.Route.RouteData = conversationContext.Route.RouteData ?? new Dictionary<string, object>();
                foreach (var data in payloadRoute?.RouteData)
                {
                    if (!conversationContext.Route.RouteData.ContainsKey(data.Key))
                    {
                        conversationContext.Route.RouteData[data.Key] = data.Value;
                    }
                }
            }
        }

        private Route PreparePayloadRoute(IConversationContext conversationContext)
        {
            Route payloadRoute = null;
            var message = conversationContext?.Entry?.Message?.Message;
            var payload = message?.Quick_reply?.Payload;
            if (payload != null)
            {
                payloadRoute = this.serializer.Deserialize<Route>(payload);
                if (payloadRoute?.RouteText != null)
                {
                    conversationContext.Entry.Message.Message.Text = payloadRoute.RouteText;
                    payloadRoute.RouteData = payloadRoute.RouteData ?? new Dictionary<string, object>();
                    if (conversationContext?.Route?.RouteData != null)
                    {
                        foreach (var data in conversationContext.Route.RouteData)
                        {
                            if (!payloadRoute.RouteData.ContainsKey(data.Key))
                            {
                                payloadRoute.RouteData[data.Key] = data.Value;
                            }
                        }
                    }

                    conversationContext.Route = null;
                }
            }

            return payloadRoute;
        }
    }
}