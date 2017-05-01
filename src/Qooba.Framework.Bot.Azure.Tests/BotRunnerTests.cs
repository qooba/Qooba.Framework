using Microsoft.Extensions.Configuration;
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
        public void RunShopping()
        {
            Run("Jad� do Arkadii chc� kupi� spodnie");
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

            string queueItem = QueueItem(message);
            BotRunner.Run(queueItem).Wait();
        }

        private string QueueItem(string message) => ("{\"connectorType\": \"Messanger\", \"message\":{\"sender\": {\"id\": \"USER_ID\"},\"recipient\": {\"id\": \"PAGE_ID\"},\"timestamp\": 1458692752478,\"message\": {\"mid\": \"mid.1457764197618:41d102a3e1ae206a38\",\"text\": \"" + message + "\",\"quick_reply\": {\"payload\": \"DEVELOPER_DEFINED_PAYLOAD\"}}}}").Replace("USER_ID", this.UserId);
    }
}
