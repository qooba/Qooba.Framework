using Qooba.Framework.Bot.Abstractions;
using Qooba.Framework.Bot.Abstractions.Models;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Qooba.Framework.Serialization.Abstractions;
using System.Text;
using System.Text.RegularExpressions;

namespace Qooba.Framework.Bot.Handlers
{
    public class RouteHandler : BaseHandler, IHandler
    {
        private static Regex rgx = new Regex("[^a-zA-Z0-9# -]", RegexOptions.Compiled);

        private readonly IEnumerable<IRouter> routers;

        private readonly ISerializer serializer;

        private readonly IDictionary<string, Route> globalRoutes;

        public RouteHandler(IEnumerable<IRouter> routers, ISerializer serializer, IRoutingConfiguration routingConfiguration)
        {
            this.routers = routers.OrderBy(x => x.Priority).ToList();
            this.serializer = serializer;
            this.globalRoutes = routingConfiguration.RoutingTable.Where(x => x.IsGlobalCommand).ToDictionary(x => RemoveAccents(x.RouteText), x => x);
        }

        public override int Priority => 2;

        public static string RemoveAccents(string text)
        {
            text = Encoding.UTF8.GetString(Encoding.GetEncoding("ISO-8859-8").GetBytes(text.ToLowerInvariant()));
            return rgx.Replace(text, "").Trim(' ');
        }

        public override async Task InvokeAsync(IConversationContext conversationContext)
        {
            var simplifiedText = RemoveAccents(conversationContext.Entry.Message.Message.Text);

            Route globalRoute;
            if (this.globalRoutes.TryGetValue(simplifiedText, out globalRoute))
            {
                var route = conversationContext.Route;
                conversationContext.Route = new Route
                {
                    RouteId = globalRoute.RouteId,
                    RouteText = globalRoute.RouteText,
                    RouteData = new Dictionary<string, object>
                    {
                        { "#lastRouteId", route?.RouteId},
                        { "#lastRouteText", route?.RouteText},
                        { "#lastRouteData", route?.RouteData},
                        { "#lastReply", conversationContext.Reply}
                    }
                };
            }
            else if (conversationContext?.Reply?.Message?.Quick_replies?.Any() == true && conversationContext?.Entry?.Message?.Message?.Quick_reply == null)
            {
                var quickReply = conversationContext.Reply.Message.Quick_replies.FirstOrDefault(x => RemoveAccents(x.Title) == simplifiedText);
                if (quickReply != null)
                {
                    conversationContext.Entry.Message.Message.Quick_reply = quickReply;
                    conversationContext.Entry.Message.Message.Text = quickReply.Title;
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
                    conversationContext.Route.RouteData[data.Key] = data.Value;
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
                    conversationContext.Reply = null;
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