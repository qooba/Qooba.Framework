using Microsoft.Extensions.Configuration;
using Moq;
using Qooba.Framework.DependencyInjection.SimpleContainer;
using System.Net.Http;
using Xunit;

namespace Qooba.Framework.Bot.Azure.Tests
{
    public class ConnectorRunnerTests
    {
        private readonly Mock<IConfigurationRoot> configurationRootMock;

        public ConnectorRunnerTests()
        {
            this.configurationRootMock = new Mock<IConfigurationRoot>();
        }

        [Fact]
        public void Run()
        {
            var container = new Container();
            container.RegisterInstance(null, typeof(IConfigurationRoot), this.configurationRootMock.Object);
            var req = new HttpRequestMessage();

            ConnectorRunner.Run(req).Wait();
        }
    }
}
