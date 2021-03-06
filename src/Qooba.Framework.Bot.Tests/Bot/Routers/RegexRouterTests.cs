﻿using Moq;
using Qooba.Framework.Bot.Abstractions;
using Qooba.Framework.Bot.Abstractions.Models;
using Qooba.Framework.Bot.Routing;
using Qooba.Framework.Bot.Common;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Xunit;
using System.Collections.Specialized;

namespace Qooba.Framework.Bot.Tests
{
    public class RegexRouterTests
    {
        private IRouter router;

        private Mock<IRoutingConfiguration> routingConfigurationMock;

        private Mock<IConversationContext> conversationContextMock;

        public RegexRouterTests()
        {
            this.conversationContextMock = new Mock<IConversationContext>();
            this.routingConfigurationMock = new Mock<IRoutingConfiguration>();
            IList<Route> routeTable = new List<Route>()
            {
                new Route { RouteId = "#hello", RouteText = "hello world"},
                new Route { RouteId = "#default", RouteText = "hello lol", IsDefault = true},
                new Route { RouteId = "#shopping", RouteText = "Jadę do {{shoppingMall}} chcę kupić {{product}}"},
                new Route { RouteId = "#accountDetails", RouteText = "Moje {{account}}"}
            };
            this.routingConfigurationMock.Setup(x => x.RoutingTable).Returns(routeTable);
            this.router = new RegexRouter(this.routingConfigurationMock.Object);
        }

        [Fact]
        public void ExactFindRouteTest()
        {
            var text = "hello world";
            PrepareContextMessage(text);
            var id = "#hello";

            var route = this.router.FindRouteAsync(this.conversationContextMock.Object).Result;

            Assert.True(route.RouteId == id);
        }

        [Fact]
        public void ShoppingFindRouteTest()
        {
            var text = "Jadę do Arkadii chcę kupić spodnie";
            PrepareContextMessage(text);
            var id = "#shopping";

            var route = this.router.FindRouteAsync(this.conversationContextMock.Object).Result;

            Assert.True(route.RouteId == id);
            Assert.True(route.RouteData["shoppingMall"].ToString() == "Arkadii");
            Assert.True(route.RouteData["product"].ToString() == "spodnie");
        }

        [Fact]
        public void AccountRouteTest()
        {
            var text = "Moje konto 360";
            PrepareContextMessage(text);
            var id = "#accountDetails";

            var route = this.router.FindRouteAsync(this.conversationContextMock.Object).Result;

            Assert.True(route.RouteId == id);
            Assert.True(route.RouteData["account"].ToString() == "konto 360");
        }

        [Fact]
        public void ReplyShufle()
        {
            var text = "[Cześć|Witaj|Siemka] Kuba [Cześć1|Witaj1|Siemka1]";
            var matches = Regex.Matches($"{text}", @"\[[a-zA-Z0-9śćźżłóę \.\-|]+\]", RegexOptions.IgnoreCase);
            foreach (var match in matches)
            {
                var t = match.ToString();
                var tt = t.TrimStart('[').TrimEnd(']').Split('|').PickRandom();
                text = text.Replace(t, tt);
            }
        }

        //[Fact]
        public void RegexMatchTest()
        {
            var exampleRoute = "Jadę do {{shoppingMall}} chcę kupić {{product}}";
            var exampleText = "Jadę do Arkadii chcę kupić spodnie";

            var parameters = Regex.Matches(exampleRoute, @"{{[a-zA-Z0-9\._]+}}");
            if (parameters.Count > 0)
            {
                var dictionary = new OrderedDictionary();
                for (int i = 0; i < parameters.Count; i++)
                {
                    var key = parameters[i].Value.Replace("{{", string.Empty).Replace("}}", string.Empty);
                    dictionary.Add(key, null);
                }

                var t = Regex.Replace(exampleRoute, @"{{[a-zA-Z0-9\._]+}}", @"([a-zA-Z0-9\._]+)");

                var match = Regex.Match(exampleText, t);
                if (match.Success)
                {
                    var groups = match.Groups;
                    for (var i = 1; i < groups.Count; i++)
                    {
                        var g = groups[i];
                        dictionary[i - 1] = g.Value;
                    }
                }
            }

        }

        private void PrepareContextMessage(string text)
        {
            this.conversationContextMock.Setup(x => x.Entry).Returns(new Entry { Message = new Messaging { Message = new EntryMessage { Text = text } } });
        }
    }
}
