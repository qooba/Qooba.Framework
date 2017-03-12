using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace AbacoosBotFunc.Tests.Handlers
{
    [TestClass]
    public class ContextKeeperHandlerTests
    {
        private IHandler contextKeeperHandler;

        private Mock<IStateManager> stateManager;

        [TestInitialize]
        public void Initialize()
        {
            this.stateManager = new Mock<IStateManager>();
            this.contextKeeperHandler = new ContextKeeperHandler(this.stateManager.Object);
        }

        [TestMethod]
        public void ContextKeepHandlingTest()
        {
            IConversationContext context = new ConversationContext
            {
                KeepState = true
            };

            this.contextKeeperHandler.InvokeAsync(context).Wait();

            Assert.IsNotNull(context);
            this.stateManager.Verify(x => x.SaveContext(context), Times.Once);
        }

        [TestMethod]
        public void ContextNoKeepHandlingTest()
        {
            IConversationContext context = new ConversationContext();

            this.contextKeeperHandler.InvokeAsync(context).Wait();

            Assert.IsNotNull(context);
            this.stateManager.Verify(x => x.SaveContext(context), Times.Never);
        }
    }
}
