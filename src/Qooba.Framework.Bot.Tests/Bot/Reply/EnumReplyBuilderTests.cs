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
    public class EnumReplyBuilderTests
    {
        private IReplyBuilder<EnumReplyMessage> replyBuilder;

        private Mock<IConversationContext> conversationContextMock;

        public EnumReplyBuilderTests()
        {
            this.conversationContextMock = new Mock<IConversationContext>();
            this.replyBuilder = new EnumReplyBuilder();
        }

        [Fact]
        public void BuildTest()
        {
            var reply = new EnumReplyMessage
            {
                Enum = new string[]
                {
                    "Red",
                    "Green",
                    "Blue"
                }
            };

            var replyMessage = this.replyBuilder.BuildAsync(this.conversationContextMock.Object, reply).Result;

            Assert.True(replyMessage.Quick_replies.Count == 3);
        }
    }
}
