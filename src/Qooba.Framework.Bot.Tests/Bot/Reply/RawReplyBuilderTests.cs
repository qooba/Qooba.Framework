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
    public class RawReplyBuilderTests
    {
        private IReplyBuilder<ReplyMessage> replyBuilder;

        private Mock<IConversationContext> conversationContextMock;

        public RawReplyBuilderTests()
        {
            this.conversationContextMock = new Mock<IConversationContext>();
            this.replyBuilder = new RawReplyBuilder();
        }
        
        [Fact]
        public void BuildTest()
        {
            ReplyMessage reply = new ReplyMessage
            {
                Text = "hello"
            };

            var replyMessage = this.replyBuilder.BuildAsync(this.conversationContextMock.Object, reply).Result;

            Assert.True(replyMessage.Text == reply.Text);
        }
    }
}
