using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using Qooba.Framework.Abstractions;
using Qooba.Framework.Bot.Abstractions;
using Qooba.Framework.Bot.Attributes;
using Xunit;
using System;
using Qooba.Framework.Bot.Abstractions.Form;
using Qooba.Framework.Bot.Abstractions.Models;

namespace Qooba.Framework.Bot.Tests.Bot
{
    public class FrameworkBotExtensionsTests
    {
        private readonly Mock<IFramework> frameworkMock;

        private readonly Mock<IFrameworkManager> frameworkManagerMock;

        private readonly Mock<IReplyConfiguration> replyConfigurationMock;

        public FrameworkBotExtensionsTests()
        {
            this.replyConfigurationMock = new Mock<IReplyConfiguration>();
            this.frameworkMock = new Mock<IFramework>();
            this.frameworkManagerMock = this.frameworkMock.As<IFrameworkManager>();
            this.frameworkMock.Setup(x => x.AddTransientService(It.IsAny<Type>(), It.IsAny<Type>())).Returns(this.frameworkMock.Object);
            this.frameworkManagerMock.Setup(x => x.GetService<IReplyConfiguration>()).Returns(this.replyConfigurationMock.Object);
        }

        [Fact]
        public void AddBotFormTest()
        {
            frameworkMock.Object.AddBotForm<TestModel>();
        }
    }

    [Route("test1")]
    [Route("test2")]
    [CompletionAction(typeof(TestReplyCompletionAction))]
    public class TestModel
    {
        [PropertyReply(typeof(NameAction))]
        [PropertyValidator(typeof(TestPropertyValidator))]
        public string Name { get; set; }
    }

    public class NameAction : IReplyAction<TextReplyMessage>
    {
        public async Task<TextReplyMessage> CreateReplyMessage(IConversationContext conversationContext, IDictionary<string, string> parameters)
        {
            return new TextReplyMessage { Text = "hello" };
        }
    }

    public class TestReplyCompletionAction : IFormReplyCompletionAction<string>
    {
        public async Task<ReplyMessage> ExecuteAsync(IConversationContext conversationContext, string completionActionData)
        {
            return new ReplyMessage();
        }
    }

    public class TestPropertyValidator : IFormReplyPropertyValidator<string>
    {
        public async Task<ReplyMessage> CheckValidAsync(IConversationContext conversationContext)
        {
            return new ReplyMessage();
        }
    }
}
