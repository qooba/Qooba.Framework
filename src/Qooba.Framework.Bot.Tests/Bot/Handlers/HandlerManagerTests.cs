using Qooba.Framework.Bot.Abstractions;
using Qooba.Framework.Bot.Handlers;
using System.Collections.Generic;
using Xunit;
using Moq;
using Qooba.Framework.Serialization.Abstractions;

namespace Qooba.Framework.Bot.Tests.Handlers
{
    public class HandlerManagerTests
    {
        private IHandlerManager handlerManager;

        private Mock<ISerializer> serializerMock;

        public HandlerManagerTests()
        {
            this.serializerMock = new Mock<ISerializer>();
        }

        [Fact]
        public void CreateTest()
        {
            var handlers = new List<IHandler>()
            {
                new DispatchHandler(null),
                new RouteHandler(new IRouter[]{ }, this.serializerMock.Object),
                new ContextHandler(null),
                new ContextKeeperHandler(null),
                new ReplyHandler(null, null)
            };

            this.handlerManager = new HandlerManager(handlers);

            var handler = this.handlerManager.CreateAsync(null).Result;

            Assert.True(handler is ContextHandler);
            Assert.True(handler.NextHandler is RouteHandler);
            Assert.True(handler.NextHandler.NextHandler is ReplyHandler);
            Assert.True(handler.NextHandler.NextHandler.NextHandler is DispatchHandler);
            Assert.True(handler.NextHandler.NextHandler.NextHandler.NextHandler is ContextKeeperHandler);
            Assert.Null(handler.NextHandler.NextHandler.NextHandler.NextHandler.NextHandler);
        }
    }
}
