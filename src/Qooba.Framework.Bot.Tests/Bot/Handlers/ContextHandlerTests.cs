using Moq;
using Qooba.Framework.Bot.Abstractions;
using Qooba.Framework.Bot.Abstractions.Models;
using Qooba.Framework.Bot.Handlers;
using Xunit;

namespace Qooba.Framework.Bot.Tests.Handlers
{
    public class ContextHandlerTests
    {
        private IHandler contextHandler;

        private Mock<IStateManager> stateManager;

        public ContextHandlerTests()
        {
            this.stateManager = new Mock<IStateManager>();
            this.contextHandler = new ContextHandler(this.stateManager.Object);
        }

        [Fact]
        public void ContextHandlingTest()
        {
            IConversationContext context = new ConversationContext();

            this.contextHandler.InvokeAsync(context).Wait();

            Assert.NotNull(context);
            this.stateManager.Verify(x => x.FetchContextAsync(context), Times.Once);
        }
    }
}
