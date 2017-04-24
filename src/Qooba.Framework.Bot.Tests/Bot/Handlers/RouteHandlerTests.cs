using Moq;
using Qooba.Framework.Bot.Abstractions;
using Qooba.Framework.Bot.Abstractions.Models;
using Qooba.Framework.Bot.Handlers;
using Xunit;

namespace Qooba.Framework.Bot.Tests.Handlers
{
    public class RouteHandlerTests
    {
        private IHandler routeHandler;

        private Mock<IRouter> routerMock;

        public RouteHandlerTests()
        {
            this.routerMock = new Mock<IRouter>();
            this.routeHandler = new RouteHandler(new[] { routerMock.Object });
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

            this.routerMock.Verify(x => x.FindRouteAsync(text), Times.Once);
        }
    }
}
