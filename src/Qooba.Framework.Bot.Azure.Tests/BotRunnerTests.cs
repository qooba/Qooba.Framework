using Microsoft.Extensions.Configuration;
using Qooba.Framework.DependencyInjection.SimpleContainer;
using Xunit;

namespace Qooba.Framework.Bot.Azure.Tests
{
    public class BotRunnerTests : BaseRunnerTests
    {
        [Fact]
        public void Run()
        {
            var container = new Container();
            container.RegisterInstance(null, typeof(IConfigurationRoot), this.configurationRootMock.Object);
            var queueItem = "{\"connectorType\": \"Messanger\", \"message\":{\"sender\": {\"id\": \"USER_ID\"},\"recipient\": {\"id\": \"PAGE_ID\"},\"timestamp\": 1458692752478,\"message\": {\"mid\": \"mid.1457764197618:41d102a3e1ae206a38\",\"text\": \"hello, world!\",\"quick_reply\": {\"payload\": \"DEVELOPER_DEFINED_PAYLOAD\"}}}}";

            BotRunner.Run(queueItem).Wait();
        }
    }
}
