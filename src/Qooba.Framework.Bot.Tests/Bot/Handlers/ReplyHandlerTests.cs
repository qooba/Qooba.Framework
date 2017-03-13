using Moq;
using Qooba.Framework.Bot.Abstractions;
using Qooba.Framework.Bot.Abstractions.Models;
using Qooba.Framework.Bot.Context;
using Qooba.Framework.Bot.Handlers;
using System.Threading.Tasks;
using Xunit;

namespace Qooba.Framework.Bot.Tests.Handlers
{
    public class ReplyHandlerTests
    {
        private IHandler replyHandler;

        private Mock<IReplyManager> replyManagerMock;
        
        public ReplyHandlerTests()
        {
            this.replyManagerMock = new Mock<IReplyManager>();
            this.replyHandler = new ReplyHandler(this.replyManagerMock.Object);
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
            this.replyManagerMock.Setup(x => x.CreateAsync(context)).Returns(Task.FromResult(new Reply
            {
                Message = new ReplyMessage
                {
                    Text = text
                }
            }));

            this.replyHandler.InvokeAsync(context).Wait();

            Assert.True(context.Reply.Message.Text == text);
            this.replyManagerMock.Verify(x => x.CreateAsync(context), Times.Once);
        }
    }
}
