using Qooba.Framework.Bot.Abstractions;
using Qooba.Framework.Bot.Abstractions.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Qooba.Framework.Bot.Routing
{
    public class RegexRouter : IRouter
    {
        private const string RegexParameterPattern = @"[a-zA-Z0-9\._ ]+";

        private readonly IList<RegexRoute> regexRoutes;

        public RegexRouter(IRoutingConfiguration routingConfiguration)
        {
            this.regexRoutes = routingConfiguration.RoutingTable.Select(x =>
            {
                var routeText = x.RouteText;
                var regexRoute = PreapreRegexRoute(routeText);
                regexRoute.IsDefault = x.IsDefault;
                regexRoute.RouteData = x.RouteData;
                regexRoute.RouteId = x.RouteId;
                regexRoute.RouteText = x.RouteText;
                return regexRoute;
            }).ToList();
        }

        public int Priority => 0;

        public async Task<Route> FindRouteAsync(IConversationContext conversationContext)
        {
            var text = conversationContext?.Entry?.Message?.Message?.Text;
            if (!string.IsNullOrEmpty(text))
            {
                foreach (var regexRoute in this.regexRoutes)
                {
                    var routeData = PrepareRouteData(text, regexRoute);
                    if (routeData != null)
                    {
                        return new Route
                        {
                            IsDefault = regexRoute.IsDefault,
                            RouteData = routeData,
                            RouteId = regexRoute.RouteId,
                            RouteText = regexRoute.RouteText
                        };
                    }
                }
            }

            return null;
        }

        public async Task<IDictionary<string, object>> FindRouteData(string text, IEnumerable<string> routeTexts)
        {
            foreach (var routeText in routeTexts)
            {
                var regexRoute = PreapreRegexRoute(routeText);
                return PrepareRouteData(text, regexRoute);
            }

            return null;
        }

        private static RegexRoute PreapreRegexRoute(string routeText)
        {
            var parameters = Regex.Matches(routeText, string.Concat(@"{{", RegexParameterPattern, "}}"));
            var regexRouteParameters = new List<string>();
            var regexRouteText = routeText;
            if (parameters.Count > 0)
            {
                for (int i = 0; i < parameters.Count; i++)
                {
                    var key = parameters[i].Value.Replace("{{", string.Empty).Replace("}}", string.Empty);
                    regexRouteParameters.Add(key);
                }

                regexRouteText = Regex.Replace(routeText, string.Concat(@"{{", RegexParameterPattern, "}}"), string.Concat(@"(", RegexParameterPattern, ")"));
            }

            return new RegexRoute
            {
                RegexRoutePattern = regexRouteText,
                RegexRouteParameters = regexRouteParameters
            };
        }

        private static IDictionary<string, object> PrepareRouteData(string text, RegexRoute regexRoute)
        {
            var match = Regex.Match(text, regexRoute.RegexRoutePattern, RegexOptions.IgnoreCase);
            if (match.Success)
            {
                var routeData = new Dictionary<string, object>();
                var groups = match.Groups;
                for (var i = 1; i < groups.Count; i++)
                {
                    var key = regexRoute.RegexRouteParameters[i - 1];
                    routeData[key] = groups[i].Value;
                }

                return routeData;
            }

            return null;
        }

        private class RegexRoute : Route
        {
            public string RegexRoutePattern { get; set; }

            public IList<string> RegexRouteParameters { get; set; }
        }
    }
}