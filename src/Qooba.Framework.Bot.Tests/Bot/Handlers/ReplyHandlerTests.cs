using Moq;
using Qooba.Framework.Bot.Abstractions;
using Qooba.Framework.Bot.Abstractions.Models;
using Qooba.Framework.Bot.Handlers;
using Qooba.Framework.Serialization.Abstractions;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Qooba.Framework.Bot.Tests.Handlers
{
    public class ReplyHandlerTests
    {
        private IHandler replyHandler;

        private Mock<IReplyConfiguration> replyConfigurationMock;

        private Mock<IReplyFactory> replyFactroyMock;

        public ReplyHandlerTests()
        {
            this.replyConfigurationMock = new Mock<IReplyConfiguration>();
            this.replyFactroyMock = new Mock<IReplyFactory>();
            this.replyHandler = new ReplyHandler(this.replyConfigurationMock.Object, this.replyFactroyMock.Object);
        }

        [Fact]
        public void ReplyHandlingTest()
        {
            var text = "hello";
            IConversationContext context = new ConversationContext
            {
                Entry = new Entry
                {
                    Message = new Messaging
                    {
                        Message = new EntryMessage
                        {
                            Text = text
                        }
                    }
                },
                Route = new Route()
            };
            var replyItem = new ReplyItem { ReplyType = "raw", Reply = new ReplyMessage() };
            this.replyConfigurationMock.Setup(x => x.FetchReplyItem(It.IsAny<IConversationContext>())).Returns(Task.FromResult(replyItem));

            this.replyHandler.InvokeAsync(context).Wait();
            
            this.replyConfigurationMock.Verify(x => x.FetchReplyItem(context), Times.Exactly(1));
            this.replyFactroyMock.Verify(x => x.CreateReplyAsync(context, replyItem), Times.Exactly(1));
        }
    }
}
