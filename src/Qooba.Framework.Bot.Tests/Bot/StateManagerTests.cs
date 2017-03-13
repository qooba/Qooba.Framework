using Moq;
using Qooba.Framework.Bot.Abstractions;
using Qooba.Framework.Bot.Context;
using Qooba.Framework.Configuration.Abstractions;
using Xunit;

namespace Qooba.Framework.Bot.Tests
{
    public class StateManagerTests
    {
        private IStateManager stateManager;

        private Mock<IConfig> configMock;
        
        public StateManagerTests()
        {
            this.configMock = new Mock<IConfig>();
            this.stateManager = new StateManager(this.configMock.Object);
        }

        [Fact]
        public void FetchContextTest()
        {
            var context = new ConversationContext();

            this.stateManager.FetchContext(context).Wait();
        }
    }
}
