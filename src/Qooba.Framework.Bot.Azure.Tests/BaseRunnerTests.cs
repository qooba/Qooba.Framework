using Microsoft.Extensions.Configuration;
using Moq;
using Newtonsoft.Json.Linq;
using System;
using System.IO;

namespace Qooba.Framework.Bot.Azure.Tests
{
    public abstract class BaseRunnerTests
    {
        protected readonly Mock<IConfigurationRoot> configurationRootMock;

        public BaseRunnerTests()
        {
            var c = File.ReadAllText("src\\Qooba.Framework.Bot.Azure.Tests\\botconfig.json");
            var config = JObject.Parse(c);
            this.configurationRootMock = new Mock<IConfigurationRoot>();
            this.configurationRootMock.Setup(x => x["InstrumentationKey"]).Returns(Guid.NewGuid().ToString());
            this.configurationRootMock.Setup(x => x["Bot::ConfigurationPath"]).Returns("src\\Qooba.Framework.Bot.Azure.Tests\\bot.json");
            this.configurationRootMock.Setup(x => x["Bot::QueueConnectionString"]).Returns(config["storageConnectionString"].ToString());
            this.configurationRootMock.Setup(x => x["Bot::Messanger::AppSecret"]).Returns("12345");
            this.configurationRootMock.Setup(x => x["Bot::QueueName"]).Returns("messanger-tests");
            this.configurationRootMock.Setup(x => x["Bot::StateManagerConnectionString"]).Returns(config["storageConnectionString"].ToString());
            this.configurationRootMock.Setup(x => x["Bot::ConversationContextTableName"]).Returns("ConversationContext");
            this.configurationRootMock.Setup(x => x["Bot::Messanger::AccessToken"]).Returns(config["accessToken"].ToString());
            this.UserId = config["userId"].ToString();
        }

        public string UserId { get; set; }
    }
}
