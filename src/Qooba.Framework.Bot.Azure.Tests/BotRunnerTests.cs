using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Qooba.Framework.Bot.Abstractions;
using Qooba.Framework.DependencyInjection.SimpleContainer;
using Xunit;

namespace Qooba.Framework.Bot.Azure.Tests
{
    public class BotRunnerTests : BaseRunnerTests
    {
        [Fact]
        public void RunHello()
        {
            Run("hello");
        }

        [Fact]
        public void RunHello1()
        {
            Run("rawattachment");
        }

        [Fact]
        public void RunShopping()
        {
            Run("Jadê do Arkadii chcê kupiæ spodnie");
        }

        [Fact]
        public void RunReplyAction()
        {
            Run("Show replyAction");
        }

        [Fact]
        public void RunText()
        {
            Run("Show text");
        }

        [Fact]
        public void RunImage()
        {
            Run("Show image");
        }

        [Fact]
        public void RunVideo()
        {
            Run("Show video");
        }

        [Fact]
        public void RunFile()
        {
            Run("Show file");
        }

        [Fact]
        public void RunEnum()
        {
            Run("Show enum");
        }

        [Fact]
        public void RunLocation()
        {
            Run("Show location");
        }

        [Fact]
        public void RunForm()
        {
            Run("Show form");
            Run("Red");
            Run("Rectangle");
        }

        [Fact]
        public void RunPostbackButtonTemplate()
        {
            Run("Show postback button template");
        }

        [Fact]
        public void RunPostbackCarousel()
        {
            Run("Show postback carousel");
        }

        private void Run(string message)
        {
            var container = new Container();
            container.RegisterInstance(null, typeof(IConfigurationRoot), this.configurationRootMock.Object);

            FrameworkBuilder.Create().AddBotAction<TestReplyAction>("testReplyAction");

            string queueItem = QueueItem(message);
            BotRunner.Run(queueItem).Wait();
        }

        private string QueueItem(string message) => ("{\"connectorType\": \"Messanger\", \"message\":{\"sender\": {\"id\": \"USER_ID\"},\"recipient\": {\"id\": \"PAGE_ID\"},\"timestamp\": 1458692752478,\"message\": {\"mid\": \"mid.1457764197618:41d102a3e1ae206a38\",\"text\": \"" + message + "\",\"quick_reply\": {\"payload\": \"DEVELOPER_DEFINED_PAYLOAD\"}}}}").Replace("USER_ID", this.UserId);
    }

    public class TestReplyAction : IReplyAction<TextReplyMessage>
    {
        public async Task<TextReplyMessage> CreateReplyMessage(IConversationContext conversationContext, IDictionary<string, string> parameters)
        {
            return new TextReplyMessage
            {
                Text = "test"
            };
        }
    }
}
