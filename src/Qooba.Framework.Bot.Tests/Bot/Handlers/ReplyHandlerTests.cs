using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;

namespace AbacoosBotFunc.Tests.Handlers
{
    [TestClass]
    public class ReplyHandlerTests
    {
        private IHandler replyHandler;

        private Mock<IReplyManager> replyManagerMock;

        [TestInitialize]
        public void Initialize()
        {
            this.replyManagerMock = new Mock<IReplyManager>();
            this.replyHandler = new ReplyHandler(this.replyManagerMock.Object);
        }

        [TestMethod]
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

            Assert.IsTrue(context.Reply.Message.Text == text);
            this.replyManagerMock.Verify(x => x.CreateAsync(context), Times.Once);
        }
    }
}
