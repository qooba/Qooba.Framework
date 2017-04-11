using Microsoft.Extensions.Configuration;
using Moq;
using Qooba.Framework.Bot.Abstractions;
using Qooba.Framework.Bot.Abstractions.Models;
using Qooba.Framework.Bot.Handlers;
using Qooba.Framework.Bot.Routing;
using Qooba.Framework.Configuration;
using Qooba.Framework.Configuration.Abstractions;
using Qooba.Framework.DependencyInjection.SimpleContainer;
using Qooba.Framework.Logging.AzureApplicationInsights;
using Qooba.Framework.Serialization;
using Qooba.Framework.Serialization.Abstractions;
using System;
using System.Collections.Generic;
using System.Net.Http;
using Xunit;

namespace Qooba.Framework.Bot.Azure.Tests
{
    public class BotRunnerTests
    {
        private readonly Mock<IConfigurationRoot> configurationRootMock;

        public BotRunnerTests()
        {
            this.configurationRootMock = new Mock<IConfigurationRoot>();
            this.configurationRootMock.Setup(x => x["InstrumentationKey"]).Returns(Guid.NewGuid().ToString());
            this.configurationRootMock.Setup(x => x["Bot::ConfigurationPath"]).Returns("src\\Qooba.Framework.Bot.Azure.Tests\\bot.json");
        }

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
