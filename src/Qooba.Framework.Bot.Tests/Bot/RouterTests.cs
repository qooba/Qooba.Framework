using Moq;
using Qooba.Framework.Bot.Abstractions;
using Qooba.Framework.Bot.Abstractions.Models;
using Qooba.Framework.Bot.Routing;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Qooba.Framework.Bot.Tests
{
    public class RouterTests
    {
        private IRouter router;

        private Mock<IRoutingConfiguration> routingConfigurationMock;
        
        public RouterTests()
        {
            this.routingConfigurationMock = new Mock<IRoutingConfiguration>();
            this.router = new Router(this.routingConfigurationMock.Object);
        }

        [Fact]
        public void ExactFindRouteTest()
        {
            var text = "hello world";
            var id = "#hello";
            IList<Route> routeTable = new List<Route>()
            {
                new Route { RouteId = id, RouteText = "hello world"},
                new Route { RouteId = "#hellolol", RouteText = "hello lol"}
            };
            this.routingConfigurationMock.Setup(x => x.RoutingTable).Returns(routeTable);

            var route = this.router.FindRouteAsync(text).Result;

            Assert.True(route.RouteId == id);
        }
    }
}
