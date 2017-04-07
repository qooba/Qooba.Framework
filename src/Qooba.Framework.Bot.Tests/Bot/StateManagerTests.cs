using Moq;
using Qooba.Framework.Bot.Abstractions;
using Qooba.Framework.Bot.Abstractions.Models;
using Qooba.Framework.Bot.Context;
using Xunit;

namespace Qooba.Framework.Bot.Tests
{
    public class StateManagerTests
    {
        private IStateManager stateManager;

        private Mock<IBotConfig> configMock;
        
        public StateManagerTests()
        {
            this.configMock = new Mock<IBotConfig>();
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
