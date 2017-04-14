using Moq;
using Qooba.Framework.Bot.Abstractions;
using Qooba.Framework.Bot.Abstractions.Models;
using Qooba.Framework.Bot.Context;
using Qooba.Framework.Bot.Handlers;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Qooba.Framework.Bot.Tests.Handlers
{
    public class ReplyHandlerTests
    {
        private IHandler replyHandler;

        private Mock<IReplyConfiguration> replyConfigurationMock;

        private Mock<IReplyBuilder> replyBuilderMock;

        public ReplyHandlerTests()
        {
            this.replyConfigurationMock = new Mock<IReplyConfiguration>();
            this.replyConfigurationMock.Setup(x => x.FetchReplyItem(It.IsAny<IConversationContext>())).Returns(Task.FromResult(new ReplyItem { ReplyType = "raw" }));
            this.replyBuilderMock = new Mock<IReplyBuilder>();
            Func<string, IReplyBuilder> builderFactory = x => this.replyBuilderMock.Object;
            this.replyHandler = new ReplyHandler(this.replyConfigurationMock.Object, builderFactory);
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
                }
            };
            this.replyBuilderMock.Setup(x => x.BuildAsync(context, It.IsAny<ReplyItem>())).Returns(Task.FromResult(new Reply
            {
                Message = new ReplyMessage
                {
                    Text = text
                }
            }));

            this.replyHandler.InvokeAsync(context).Wait();

            Assert.True(context.Reply.Message.Text == text);
            this.replyConfigurationMock.Verify(x => x.FetchReplyItem(context), Times.Once);
            this.replyBuilderMock.Verify(x => x.BuildAsync(context, It.IsAny<ReplyItem>()), Times.Once);
        }
    }
}
