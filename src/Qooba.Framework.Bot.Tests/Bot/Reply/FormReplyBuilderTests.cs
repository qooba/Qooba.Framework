using Moq;
using Qooba.Framework.Bot.Abstractions;
using Xunit;

namespace Qooba.Framework.Bot.Tests
{
    public class FormReplyBuilderTests
    {
        private IReplyBuilder<FormReplyMessage> replyBuilder;

        private Mock<IConversationContext> conversationContextMock;

        private Mock<IReplyFactory> replyFactoryMock;

        public FormReplyBuilderTests()
        {
            this.conversationContextMock = new Mock<IConversationContext>();
            this.replyFactoryMock = new Mock<IReplyFactory>();
            this.replyBuilder = new FormReplyBuilder(this.replyFactoryMock.Object);
        }

        [Fact]
        public void BuildTest()
        {
            var reply = new FormReplyMessage
            {
                
            };

            var replyMessage = this.replyBuilder.BuildAsync(this.conversationContextMock.Object, reply).Result;

            //Assert.True(replyMessage.Text == reply.Text);
        }
    }
}
