using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AbacoosBotFunc.Tests.Handlers
{
    [TestClass]
    public class ReplyManagerTests
    {
        private IReplyManager replyManager;

        private Mock<IConfig> configMock;

        private Mock<IReplyBuilder> replyBuilderMock;

        [TestInitialize]
        public void Initialize()
        {
            this.configMock = new Mock<IConfig>();
            this.configMock.Setup(x => x.Get(Constants.BotConfigurationPath)).Returns("Bot/bot.json");
            this.replyBuilderMock = new Mock<IReplyBuilder>();
            Func<string, IReplyBuilder> func = s => s == "raw" ? replyBuilderMock.Object : null;
            this.replyManager = new ReplyManager(this.configMock.Object, func);
        }

        [TestMethod]
        public void FetchRoutingTableTest()
        {
            var routingTable = this.replyManager.FetchRoutingTableAsync().Result;

            Assert.IsTrue(routingTable.Count == 2);
        }

        [TestMethod]
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

            Assert.IsTrue(reply.Message.Text == text);
            this.replyBuilderMock.Verify(x => x.BuildAsync(contextMock.Object, It.IsAny<ReplyItem>()), Times.Once);
        }
    }
}
