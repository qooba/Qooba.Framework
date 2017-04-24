using Moq;
using Qooba.Framework.Bot.Abstractions;
using Qooba.Framework.Bot.Abstractions.Models;
using Qooba.Framework.Bot.Routing;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xunit;
using System.Linq;
using System.Collections.Specialized;

namespace Qooba.Framework.Bot.Tests
{
    public class DefaultRouterTests
    {
        private IRouter router;

        private Mock<IRoutingConfiguration> routingConfigurationMock;

        public DefaultRouterTests()
        {
            this.routingConfigurationMock = new Mock<IRoutingConfiguration>();
            IList<Route> routeTable = new List<Route>()
            {
                new Route { RouteId = "#hello", RouteText = "hello world"},
                new Route { RouteId = "#default", RouteText = "hello lol", IsDefault = true},
                new Route { RouteId = "#shopping", RouteText = "Jadę do {{shoppingMall}} chcę kupić {{product}}"}
            };
            this.routingConfigurationMock.Setup(x => x.RoutingTable).Returns(routeTable);
            this.router = new DefaultRouter(this.routingConfigurationMock.Object);
        }
        
        [Fact]
        public void DefaultFindRouteTest()
        {
            var text = "some text";
            var id = "#hello";
            
            var route = this.router.FindRouteAsync(text).Result;

            Assert.True(route.RouteId == id);
        }
    }
}
