using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace AbacoosBotFunc.Tests.Handlers
{
    [TestClass]
    public class DispatchHandlerTests
    {
        private IHandler dispatchHandler;

        private Mock<IDispatcher> dispatcherMock;

        [TestInitialize]
        public void Initialize()
        {
            this.dispatcherMock = new Mock<IDispatcher>();
            this.dispatchHandler = new DispatchHandler(this.dispatcherMock.Object);
        }

        [TestMethod]
        public void DispatchHandlingTest()
        {
            var reply = new Reply
            {
                Message = new ReplyMessage
                {
                    Text = "hello"
                }
            };

            IConversationContext context = new ConversationContext
            {
                Reply = reply
            };

            this.dispatchHandler.InvokeAsync(context).Wait();

            this.dispatcherMock.Verify(x => x.SendAsync(reply), Times.Once);
        }
    }
}
