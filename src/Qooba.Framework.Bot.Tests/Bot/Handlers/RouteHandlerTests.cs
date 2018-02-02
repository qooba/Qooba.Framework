using Moq;
using Qooba.Framework.Bot.Abstractions;
using Qooba.Framework.Bot.Abstractions.Models;
using Qooba.Framework.Bot.Handlers;
using Qooba.Framework.Serialization.Abstractions;
using Xunit;

namespace Qooba.Framework.Bot.Tests.Handlers
{
    public class RouteHandlerTests
    {
        private IHandler routeHandler;

        private Mock<IRouter> routerMock;

        private Mock<ISerializer> serializerMock;

        public RouteHandlerTests()
        {
            this.serializerMock = new Mock<ISerializer>();
            this.routerMock = new Mock<IRouter>();
            this.routeHandler = new RouteHandler(new[] { routerMock.Object }, this.serializerMock.Object);
        }

        [Fact]
        public void FindRouteHandlingTest()
        {
            var text = "hello";
            IConversationContext context = new ConversationContext
            {
                Entry = new Entry
                {
                    Message = new Messaging
                    {
                        Message = new EntryMessage
                        {
                            Text = text
                        }
                    }
                }
            };

            this.routeHandler.InvokeAsync(context).Wait();

            this.routerMock.Verify(x => x.FindRouteAsync(context), Times.Once);
        }
    }
}
