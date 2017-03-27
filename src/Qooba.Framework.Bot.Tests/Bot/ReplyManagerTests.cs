using Moq;
using Qooba.Framework.Bot.Abstractions;
using Qooba.Framework.Bot.Abstractions.Models;
using Qooba.Framework.Configuration.Abstractions;
using Qooba.Framework.Serialization.Abstractions;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Qooba.Framework.Bot.Tests
{
    public class ReplyManagerTests
    {
        private IReplyManager replyManager;

        private Mock<IConfig> configMock;

        private Mock<IReplyBuilder> replyBuilderMock;

        private Mock<ISerializer> seriazlierMock;

        public ReplyManagerTests()
        {
            this.configMock = new Mock<IConfig>();
            this.configMock.Setup(x => x[Constants.BotConfigurationPath]).Returns("Bot/bot.json");
            this.replyBuilderMock = new Mock<IReplyBuilder>();
            this.seriazlierMock = new Mock<ISerializer>();
            this.seriazlierMock.Setup(x => x.Deserialize<ReplyConfiguration>(It.IsAny<string>())).Returns(new ReplyConfiguration
            {
                Items = new []
                {
                    new ReplyItem
                    {
                        Routes = new [] {"hello", "test" },
                        ReplyType = "raw"
                    }
                }
            });

            Func<string, IReplyBuilder> func = s => s == "raw" ? replyBuilderMock.Object : null;
            this.replyManager = new ReplyManager(this.configMock.Object, this.seriazlierMock.Object, func);
        }

        [Fact]
        public void FetchRoutingTableTest()
        {
            var routingTable = this.replyManager.FetchRoutingTableAsync().Result;

            Assert.True(routingTable.Count == 2);
        }

        [Fact]
        public void CreateReplyTest()
        {
            var routeId = "hello";
            var text = "hello";
            Mock<IConversationContext> contextMock = new Mock<IConversationContext>();
            contextMock.Setup(x => x.Route).Returns(new Route
            {
                RouteId = routeId
            });
            this.replyBuilderMock.Setup(x => x.BuildAsync(contextMock.Object, It.IsAny<ReplyItem>())).Returns(Task.FromResult(new Reply
            {
                Message = new ReplyMessage
                {
                    Text = text
                }
            }));

            var reply = this.replyManager.CreateAsync(contextMock.Object).Result;

            Assert.True(reply.Message.Text == text);
            this.replyBuilderMock.Verify(x => x.BuildAsync(contextMock.Object, It.IsAny<ReplyItem>()), Times.Once);
        }
    }
}
