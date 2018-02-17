using Moq;
using Qooba.Framework.Bot.Abstractions;
using Qooba.Framework.Abstractions;
using Qooba.Framework.Serialization.Abstractions;
using System.Threading.Tasks;
using Xunit;

namespace Qooba.Framework.Bot.Tests
{
    public class BotTests
    {
        private QBot bot;

        private Mock<ILogger> loggerMock;

        private Mock<IHandlerManager> handlerManagerMock;

        private Mock<IHandler> handlerMock;

        private Mock<ISerializer> serializerMock;

        public BotTests()
        {
            this.loggerMock = new Mock<ILogger>();
            this.handlerManagerMock = new Mock<IHandlerManager>();
            this.handlerMock = new Mock<IHandler>();
            this.handlerManagerMock.Setup(x => x.CreateAsync(It.IsAny<IConversationContext>())).Returns(Task.FromResult(this.handlerMock.Object));
            this.serializerMock = new Mock<ISerializer>();
            this.bot = new QBot(this.loggerMock.Object, this.handlerManagerMock.Object, this.serializerMock.Object);
        }

        [Fact]
        public void QueueItemDeserializationTest()
        {
            var queueItem = "{\"connectorType\": \"Messanger\", \"message\":{\"sender\": {\"id\": \"USER_ID\"},\"recipient\": {\"id\": \"PAGE_ID\"},\"timestamp\": 1458692752478,\"message\": {\"mid\": \"mid.1457764197618:41d102a3e1ae206a38\",\"text\": \"hello, world!\",\"quick_reply\": {\"payload\": \"DEVELOPER_DEFINED_PAYLOAD\"}}}}";

            this.bot.Run(queueItem).Wait();

            this.handlerManagerMock.Verify(x => x.CreateAsync(It.IsAny<IConversationContext>()));
            this.handlerMock.Verify(x => x.InvokeAsync(It.IsAny<IConversationContext>()), Times.Once);
        }
    }
}
