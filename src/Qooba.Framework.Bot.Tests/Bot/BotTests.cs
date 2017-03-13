using Moq;
using Qooba.Framework.Bot.Abstractions;
using Qooba.Framework.Logging.Abstractions;
using Xunit;

namespace Qooba.Framework.Bot.Tests
{
    public class BotTests
    {
        private Bot bot;
        
        private Mock<ILogger> loggerMock;

        private Mock<IHandler> handlerMock;
        
        public BotTests()
        {
            this.loggerMock = new Mock<ILogger>();
            this.handlerMock = new Mock<IHandler>();
            this.bot = new Bot(this.loggerMock.Object, this.handlerMock.Object);
        }

        [Fact]
        public void QueueItemDeserializationTest()
        {
            var queueItem = "{\"connectorType\": \"Messanger\", \"message\":{\"sender\": {\"id\": \"USER_ID\"},\"recipient\": {\"id\": \"PAGE_ID\"},\"timestamp\": 1458692752478,\"message\": {\"mid\": \"mid.1457764197618:41d102a3e1ae206a38\",\"text\": \"hello, world!\",\"quick_reply\": {\"payload\": \"DEVELOPER_DEFINED_PAYLOAD\"}}}}";

            this.bot.Run(queueItem).Wait();

            this.handlerMock.Verify(x => x.InvokeAsync(It.IsAny<IConversationContext>()), Times.Once);
        }
    }
}
