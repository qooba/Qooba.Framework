using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace AbacoosBotFunc.Tests
{
    [TestClass]
    public class BotTests
    {
        private Bot bot;

        private Mock<ITelemetry> telemetryMock;

        private Mock<ILogger> loggerMock;

        private Mock<IHandler> handlerMock;

        [TestInitialize]
        public void Initialize()
        {
            this.loggerMock = new Mock<ILogger>();
            this.telemetryMock = new Mock<ITelemetry>();
            this.handlerMock = new Mock<IHandler>();
            this.bot = new Bot(this.telemetryMock.Object, this.loggerMock.Object, this.handlerMock.Object);
        }

        [TestMethod]
        public void QueueItemDeserializationTest()
        {
            var queueItem = "{\"connectorType\": \"Messanger\", \"message\":{\"sender\": {\"id\": \"USER_ID\"},\"recipient\": {\"id\": \"PAGE_ID\"},\"timestamp\": 1458692752478,\"message\": {\"mid\": \"mid.1457764197618:41d102a3e1ae206a38\",\"text\": \"hello, world!\",\"quick_reply\": {\"payload\": \"DEVELOPER_DEFINED_PAYLOAD\"}}}}";

            this.bot.Run(queueItem).Wait();

            this.handlerMock.Verify(x => x.InvokeAsync(It.IsAny<IConversationContext>()), Times.Once);
        }
    }
}
