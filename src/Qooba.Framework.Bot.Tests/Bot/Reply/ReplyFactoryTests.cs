using Moq;
using Qooba.Framework.Bot.Abstractions;
using Qooba.Framework.Bot.Abstractions.Models;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Qooba.Framework.Bot.Tests.Handlers
{
    public class ReplyFactoryTests
    {
        private IReplyFactory replyFactory;

        private Mock<IReplyBuilder<ReplyMessage>> replyBuilderMock;

        private Mock<IGenericExpressionFactory> genericExpressionFactoryMock;

        private readonly Func<object, IReplyBuilder> builderFactory;

        public ReplyFactoryTests()
        {
            this.replyBuilderMock = new Mock<IReplyBuilder<ReplyMessage>>();
            this.genericExpressionFactoryMock = new Mock<IGenericExpressionFactory>();

            this.builderFactory = x => this.replyBuilderMock.Object;

            this.replyFactory = new ReplyFactory(builderFactory, this.genericExpressionFactoryMock.Object);
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
            this.genericExpressionFactoryMock.Setup(x => x.Create(It.Is<string>(s => s == "raw"), builderFactory, context, It.IsAny<string>())).
                Returns(Task.FromResult(new ReplyMessage
                {
                    Text = text
                }));
            
            this.replyFactory.CreateReplyAsync(context, replyItem).Wait();
            var reply = this.replyFactory.CreateReplyAsync(context, replyItem).Result;

            Assert.True(reply.Message.Text == text);
            this.genericExpressionFactoryMock.VerifyAll();
        }
    }
}
