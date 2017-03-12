using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AbacoosBotFunc.Tests.Handlers
{
    [TestClass]
    public class RouterTests
    {
        private IRouter router;

        private Mock<IReplyManager> replyManagerMock;

        [TestInitialize]
        public void Initialize()
        {
            this.replyManagerMock = new Mock<IReplyManager>();
            this.router = new Router(this.replyManagerMock.Object);
        }

        [TestMethod]
        public void ExactFindRouteTest()
        {
            var text = "hello world";
            var id = "#hello";
            IList<Route> routeTable = new List<Route>()
            {
                new Route { RouteId = id, RouteText = "hello world"},
                new Route { RouteId = "#hellolol", RouteText = "hello lol"}
            };
            this.replyManagerMock.Setup(x => x.FetchRoutingTableAsync()).Returns(Task.FromResult(routeTable));

            var route = this.router.FindRouteAsync(text).Result;

            Assert.IsTrue(route.RouteId == id);
        }
    }
}
