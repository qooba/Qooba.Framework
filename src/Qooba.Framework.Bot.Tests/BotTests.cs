using Moq;
using Qooba.Framework.Bot.Abstractions;
using Qooba.Framework.Bot.Connector.Abstractions;
using Qooba.Framework.Bot.Connector.Abstractions.Model;
using Qooba.Framework.DependencyInjection.Abstractions;
using Qooba.Framework.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Qooba.Framework.Bot.Tests
{
    public class BotTests
    {
        private readonly IBot bot;

        private readonly Mock<ILogger> loggerMock;

        private readonly Mock<Func<string, IConnector>> connectorFactoryMock;

        public BotTests()
        {
            this.connectorFactoryMock = new Mock<Func<string,IConnector>>();
            this.loggerMock = new Mock<ILogger>();
            this.bot = new BotImpl(this.connectorFactoryMock.Object, this.loggerMock.Object);
        }

        [Fact]
        public async Task ProcessSuccessTest()
        {
            var path = "messanger";
            var connectorMock = new Mock<IConnector>();
            this.connectorFactoryMock.Setup(x => x(It.IsAny<string>())).Returns(connectorMock.Object);

            await this.bot.ProcessAsync(path, new Dictionary<string, string[]>(), "callback");

            this.loggerMock.Verify(x => x.Error(It.IsAny<string>()), Times.Never);
            this.connectorFactoryMock.Verify(x => x(ConnectorType.Messanger.ToString()), Times.Once);
        }

        [Fact]
        public async Task ProcessErrorTest()
        {
            var path = "error";

            await this.bot.ProcessAsync(path, new Dictionary<string, string[]>(), "callback");

            this.loggerMock.Verify(x => x.Error(It.IsAny<string>()), Times.Once);
            this.connectorFactoryMock.Verify(x => x(It.IsAny<string>()), Times.Never);
        }
    }
}
