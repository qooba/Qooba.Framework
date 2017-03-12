using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AbacoosBotFunc.Tests
{
    [TestClass]
    public class StateManagerTests
    {
        private IStateManager stateManager;

        private Mock<IConfig> configMock;

        [TestInitialize]
        public void Initialize()
        {
            this.configMock = new Mock<IConfig>();
            this.stateManager = new StateManager(this.configMock.Object);
        }

        [TestMethod]
        public void FetchContextTest()
        {
            var context = new ConversationContext();

            this.stateManager.FetchContext(context).Wait();
        }
    }
}
