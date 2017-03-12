using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AbacoosBotFunc.Tests.Handlers
{
    [TestClass]
    public class HandlerManagerTests
    {
        private IHandlerManager handlerManager;

        [TestInitialize]
        public void Initialize()
        {

        }

        [TestMethod]
        public void CreateTest()
        {
            var handlers = new List<IHandler>()
            {
                new DispatchHandler(null),
                new RouteHandler(null),
                new ContextHandler(null),
                new ContextKeeperHandler(null),
                new ReplyHandler(null)
            };

            this.handlerManager = new HandlerManager(handlers);

            var handler = this.handlerManager.CreateAsync(null).Result;

            Assert.IsTrue(handler is ContextHandler);
            Assert.IsTrue(handler.NextHandler is RouteHandler);
            Assert.IsTrue(handler.NextHandler.NextHandler is ReplyHandler);
            Assert.IsTrue(handler.NextHandler.NextHandler.NextHandler is DispatchHandler);
            Assert.IsTrue(handler.NextHandler.NextHandler.NextHandler.NextHandler is ContextKeeperHandler);
            Assert.IsNull(handler.NextHandler.NextHandler.NextHandler.NextHandler.NextHandler);
        }
    }
}
