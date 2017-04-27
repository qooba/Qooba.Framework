using Moq;
using Qooba.Framework.Bot.Abstractions;
using Qooba.Framework.Bot.Abstractions.Models;
using Qooba.Framework.Serialization.Abstractions;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Qooba.Framework.Bot.Tests.Handlers
{
    public class ReplyFactoryTests
    {
        private IReplyFactory replyFactory;
        
        private Mock<IReplyBuilder<ReplyMessage>> replyBuilderMock;

        private Mock<ISerializer> serializerMock;

        public ReplyFactoryTests()
        {
            this.replyBuilderMock = new Mock<IReplyBuilder<ReplyMessage>>();
            this.serializerMock = new Mock<ISerializer>();
            
            Func<object, IReplyBuilder> builderFactory = x => this.replyBuilderMock.Object;
            this.replyFactory = new ReplyFactory(builderFactory, serializerMock.Object);
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
            this.replyBuilderMock.Setup(x => x.BuildAsync(context, It.IsAny<ReplyMessage>())).Returns(Task.FromResult(new ReplyMessage
            {
                Text = text
            }));
            this.serializerMock.Setup(x => x.Deserialize(It.IsAny<string>(), It.IsAny<Type>())).Returns(new ReplyMessage { Text = text });

            this.replyFactory.CreateReplyAsync(context, replyItem).Wait();
            var reply = this.replyFactory.CreateReplyAsync(context, replyItem).Result;

            Assert.True(reply.Message.Text == text);
            this.replyBuilderMock.Verify(x => x.BuildAsync(context, It.IsAny<ReplyMessage>()), Times.Exactly(2));
        }
    }
}
