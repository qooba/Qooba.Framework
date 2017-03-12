using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace AbacoosBotFunc.Tests.Handlers
{
    [TestClass]
    public class ContextHandlerTests
    {
        private IHandler contextHandler;

        private Mock<IStateManager> stateManager;

        [TestInitialize]
        public void Initialize()
        {
            this.stateManager = new Mock<IStateManager>();
            this.contextHandler = new ContextHandler(this.stateManager.Object);
        }

        [TestMethod]
        public void ContextHandlingTest()
        {
            IConversationContext context = new ConversationContext();

            this.contextHandler.InvokeAsync(context).Wait();

            Assert.IsNotNull(context);
            this.stateManager.Verify(x => x.FetchContext(context), Times.Once);
        }
    }
}
