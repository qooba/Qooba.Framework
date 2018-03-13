using Moq;
using Qooba.Framework.Abstractions;
using Qooba.Framework.Bot.Abstractions;
using Qooba.Framework.Bot.Abstractions.Models;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Qooba.Framework.Bot.Tests.Handlers
{
    public class GenericExpressionFactoryTests
    {
        private readonly IGenericExpressionFactory genericExpressionFactory;

        private readonly Mock<ISerializer> serializerMock;

        public GenericExpressionFactoryTests()
        {
            this.serializerMock = new Mock<ISerializer>();
            this.genericExpressionFactory = new GenericExpressionFactory(this.serializerMock.Object);
        }

        [Fact]
        public void CreateTest()
        {
            Func<string, IReplyBuilder> replyBuilderFactory = x => new TextReplyBuilder();
            var conversationContextMock = new Mock<IConversationContext>();
            var replyData = "{ \"test\": \"hello\" }";
            this.serializerMock.Setup(x => x.Deserialize(It.IsAny<string>(), It.IsAny<Type>())).Returns(new TextReplyMessage
            {
                Text = "hello"
            });

            var reply = ((Task<ReplyMessage>)this.genericExpressionFactory.Create("text", replyBuilderFactory, conversationContextMock.Object, replyData)).Result;

            Assert.True(reply != null && reply.Text == "hello");
        }
    }
}
