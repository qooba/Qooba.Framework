using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace AbacoosBotFunc.Tests.Handlers
{
    [TestClass]
    public class RouteHandlerTests
    {
        private IHandler routeHandler;

        private Mock<IRouter> routerMock;

        [TestInitialize]
        public void Initialize()
        {
            this.routerMock = new Mock<IRouter>();
            this.routeHandler = new RouteHandler(this.routerMock.Object);
        }

        [TestMethod]
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
