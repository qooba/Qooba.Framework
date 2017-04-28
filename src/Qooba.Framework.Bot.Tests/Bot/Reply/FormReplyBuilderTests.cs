using Moq;
using Qooba.Framework.Bot.Abstractions;
using Qooba.Framework.Bot.Abstractions.Models;
using Qooba.Framework.Bot.Routing;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xunit;
using System.Linq;
using System.Collections.Specialized;

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
