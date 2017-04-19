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

        private Mock<IReplyBuilder<ReplyMessage>> replyBuilderMock;

        private Mock<ISerializer> serializerMock;

        public ReplyHandlerTests()
        {
            this.replyConfigurationMock = new Mock<IReplyConfiguration>();
            this.replyConfigurationMock.Setup(x => x.FetchReplyItem(It.IsAny<IConversationContext>())).Returns(Task.FromResult(new ReplyItem { ReplyType = "raw", Reply = new ReplyMessage()  }));
            this.replyBuilderMock = new Mock<IReplyBuilder<ReplyMessage>>();
            this.serializerMock = new Mock<ISerializer>();
            
            Func<string, IReplyBuilder> builderFactory = x => this.replyBuilderMock.Object;
            this.replyHandler = new ReplyHandler(this.replyConfigurationMock.Object, builderFactory, serializerMock.Object);
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
            this.replyBuilderMock.Setup(x => x.BuildAsync(context, It.IsAny<ReplyMessage>())).Returns(Task.FromResult(new ReplyMessage
            {
                Text = text
            }));
            this.serializerMock.Setup(x => x.Deserialize(It.IsAny<string>(), It.IsAny<Type>())).Returns(new ReplyMessage { Text = text });

            this.replyHandler.InvokeAsync(context).Wait();
            this.replyHandler.InvokeAsync(context).Wait();

            Assert.True(context.Reply.Message.Text == text);
            this.replyConfigurationMock.Verify(x => x.FetchReplyItem(context), Times.Exactly(2));
            this.replyBuilderMock.Verify(x => x.BuildAsync(context, It.IsAny<ReplyMessage>()), Times.Exactly(2));
        }
    }
}
