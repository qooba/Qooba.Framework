using System.Collections.Generic;
using System.Linq;

namespace Qooba.Framework.Bot.Abstractions.Models
{
    public class QuickReplyPayload
    {
        private readonly string routeText;

        private readonly IDictionary<string, object> routeData;

        public QuickReplyPayload(Route route)
        {
            this.routeText = route.RouteText;
            this.routeData = route.RouteData;
        }

        public QuickReplyPayload(string routeText, IDictionary<string, object> routeData)
        {
            this.routeText = routeText;
            this.routeData = routeData;
        }

        public QuickReplyPayload(string routeText, IDictionary<string, object> routeData, IConversationContext conversationContext)
        {
            this.routeText = routeText ?? conversationContext.Route.RouteText;
            this.routeData = routeData ?? conversationContext?.Route?.RouteData;
            if (routeData != null && conversationContext?.Route?.RouteData?.Any() == true)
            {
                foreach (var data in conversationContext?.Route?.RouteData)
                {
                    if (!this.routeData.ContainsKey(data.Key))
                    {
                        this.routeData[data.Key] = data.Value;
                    }
                }
            }
        }

        public QuickReplyPayload(string routeText, IConversationContext conversationContext) :
            this(routeText, null, conversationContext)
        {
        }

        public QuickReplyPayload(IConversationContext conversationContext) :
                this(null, conversationContext)
        {
        }

        public override string ToString()
        {
            var routeTextPart = this.routeText != null ? $"\"routeText\": \"{this.routeText}\" " : string.Empty;
            var sep = this.routeData?.Any() == true ? "," : string.Empty;
            var routeDataPart = this.routeData?.Any() == true ? $"\"routeData\": {{ {string.Join(",", this.routeData.Select(x => $"\"{x.Key}\" : \"{x.Value}\" "))} }}" : string.Empty;
            return $"{{ {routeTextPart} {sep} {routeDataPart} }}";
        }
    }
}