using Microsoft.Extensions.Configuration;
using Moq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Xunit;

namespace Qooba.Framework.Bot.Azure.Tests
{
    public class ConnectorRunnerTests : BaseRunnerTests
    {
        [Fact]
        public void Run()
        {
            var container = new Container();
            container.RegisterInstance(null, typeof(IConfigurationRoot), this.configurationRootMock.Object);

            var req = new HttpRequestMessage
            {
                RequestUri = new System.Uri("http://qooba.net/bot?q=test&hub.mode=run"),
                Content = new StringContent("{\"object\":\"page\",\"entry\":[{\"id\":\"1\",\"time\":2,\"messaging\":[{\"sender\":{\"id\":\"3\"},\"recipient\":{\"id\":\"4\"},\"timestamp\":5,\"delivery\":{\"mids\":[\"mid.5:7\"],\"watermark\":8,\"seq\":0}}]}]}")
            };

            req.Headers.Add("X-Hub-Signature", "xxxx-d3f107f0b05d357695721f177fc11a5384a5afa9");

            ConnectorRunner.Run(req).Wait();
        }
    }
}
