using Moq;
using Qooba.Framework.Bot.Abstractions;
using Qooba.Framework.Bot.Abstractions.Models;
using Qooba.Framework.Serialization.Abstractions;
using Xunit;

namespace Qooba.Framework.Bot.Tests
{
    public class ReplyManagerTests
    {
        private IReplyConfiguration replyConfiguration;

        private IRoutingConfiguration routingConfiguration;

        private Mock<IBotConfig> configMock;

        private Mock<IReplyBuilder> replyBuilderMock;

        private Mock<ISerializer> seriazlierMock;

        public ReplyManagerTests()
        {
            this.configMock = new Mock<IBotConfig>();
            this.configMock.Setup(x => x.BotConfigurationPath).Returns("Bot/bot.json");
            this.replyBuilderMock = new Mock<IReplyBuilder>();
            this.seriazlierMock = new Mock<ISerializer>();
            this.seriazlierMock.Setup(x => x.Deserialize<ReplyConfiguration>(It.IsAny<string>())).Returns(new ReplyConfiguration
            {
                Items = new[]
                {
                    new ReplyItem
                    {
                        ReplyId = "hello",
                        Routes = new [] {"hello", "test" },
                        ReplyType = "raw"
                    }
                }
            });

            var replyManager = new ReplyManager(this.configMock.Object, this.seriazlierMock.Object);
            this.routingConfiguration = replyManager;
            this.replyConfiguration = replyManager;
        }

        [Fact]
        public void FetchRoutingTableTest()
        {
            var routingTable = this.routingConfiguration.RoutingTable;
            Assert.True(routingTable.Count == 2);
        }

        [Fact]
        public void FetchReplyItemTest()
        {
            var routeId = "hello";
            var text = "hello";
            Mock<IConversationContext> contextMock = new Mock<IConversationContext>();
            contextMock.Setup(x => x.Route).Returns(new Route
            {
                RouteId = routeId
            });

            var replyItem = this.replyConfiguration.FetchReplyItem(contextMock.Object).Result;

            Assert.True(replyItem.ReplyType == "raw");
        }
    }
}
