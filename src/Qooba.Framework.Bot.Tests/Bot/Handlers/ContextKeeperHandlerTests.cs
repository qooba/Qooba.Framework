using Moq;
using Qooba.Framework.Bot.Abstractions;
using Qooba.Framework.Bot.Abstractions.Models;
using Qooba.Framework.Bot.Handlers;
using Xunit;

namespace Qooba.Framework.Bot.Tests.Handlers
{
    public class ContextKeeperHandlerTests
    {
        private IHandler contextKeeperHandler;

        private Mock<IStateManager> stateManager;
        
        public ContextKeeperHandlerTests()
        {
            this.stateManager = new Mock<IStateManager>();
            this.contextKeeperHandler = new ContextKeeperHandler(this.stateManager.Object);
        }

        [Fact]
        public void ContextKeepHandlingTest()
        {
            IConversationContext context = new ConversationContext
            {
                KeepState = true
            };

            this.contextKeeperHandler.InvokeAsync(context).Wait();

            Assert.NotNull(context);
            this.stateManager.Verify(x => x.SaveContextAsync(context), Times.Once);
        }

        [Fact]
        public void ContextNoKeepHandlingTest()
        {
            IConversationContext context = new ConversationContext();

            this.contextKeeperHandler.InvokeAsync(context).Wait();

            Assert.NotNull(context);
            this.stateManager.Verify(x => x.SaveContextAsync(context), Times.Never);
        }
    }
}
