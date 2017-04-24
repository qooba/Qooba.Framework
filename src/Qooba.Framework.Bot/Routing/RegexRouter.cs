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
        private readonly IList<RegexRoute> regexRoutes;

        public RegexRouter(IRoutingConfiguration routingConfiguration)
        {
            this.regexRoutes = routingConfiguration.RoutingTable.Select(x =>
            {
                var parameters = Regex.Matches(x.RouteText, @"{{[a-zA-Z0-9\._]+}}");
                var regexRouteParameters = new List<string>();
                var regexRouteText = x.RouteText;
                if (parameters.Count > 0)
                {
                    for (int i = 0; i < parameters.Count; i++)
                    {
                        var key = parameters[i].Value.Replace("{{", string.Empty).Replace("}}", string.Empty);
                        regexRouteParameters.Add(key);
                    }

                    regexRouteText = Regex.Replace(x.RouteText, @"{{[a-zA-Z0-9\._]+}}", @"([a-zA-Z0-9\._]+)");
                }

                return new RegexRoute
                {
                    IsDefault = x.IsDefault,
                    RouteData = x.RouteData,
                    RouteId = x.RouteId,
                    RouteText = x.RouteText,
                    RegexRoutePattern = regexRouteText,
                    RegexRouteParameters = regexRouteParameters
                };
            }).ToList();
        }

        public int Priority => 0;

        public async Task<Route> FindRouteAsync(string text)
        {
            foreach (var regexRoute in this.regexRoutes)
            {
                var match = Regex.Match(text, regexRoute.RegexRoutePattern, RegexOptions.IgnoreCase);
                if (match.Success)
                {
                    var route = new Route
                    {
                        IsDefault = regexRoute.IsDefault,
                        RouteData = new Dictionary<string, object>(),
                        RouteId = regexRoute.RouteId,
                        RouteText = regexRoute.RouteText
                    };

                    var groups = match.Groups;
                    for (var i = 1; i < groups.Count; i++)
                    {
                        var key = regexRoute.RegexRouteParameters[i - 1];
                        route.RouteData[key] = groups[i].Value;
                    }

                    return route;
                }
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