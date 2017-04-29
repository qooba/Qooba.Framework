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
    public class LocationReplyBuilderTests
    {
        private IReplyBuilder<LocationReplyMessage> replyBuilder;

        private Mock<IConversationContext> conversationContextMock;

        public LocationReplyBuilderTests()
        {
            this.conversationContextMock = new Mock<IConversationContext>();
            this.replyBuilder = new LocationReplyBuilder();
        }

        [Fact]
        public void BuildTest()
        {
            var reply = new LocationReplyMessage
            {
                Text = "location"
            };

            var replyMessage = this.replyBuilder.BuildAsync(this.conversationContextMock.Object, reply).Result;

            Assert.True(replyMessage.Quick_replies.Count == 1);
        }
    }
}
