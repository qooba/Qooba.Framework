using Microsoft.AspNetCore.Http;
using Moq;
using Qooba.Framework.Bot.Abstractions;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Qooba.Framework.Bot.Tests
{
    public class BotMiddlewareTests
    {
        private readonly Mock<IConnector> connectorMock;

        private readonly Mock<RequestDelegate> requestDelegateMock;

        private readonly BotMiddleware botMiddleware;

        public BotMiddlewareTests()
        {
            this.connectorMock = new Mock<IConnector>();
            this.requestDelegateMock = new Mock<RequestDelegate>();
            this.botMiddleware = new BotMiddleware(this.requestDelegateMock.Object);
        }

        [Fact]
        public async Task InvokeTest()
        {
            var context = new Mock<HttpContext>();
            var request = new Mock<HttpRequest>();
            var response = new Mock<HttpResponse>();
            //response.Setup(x => x.WriteAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            var data = Encoding.UTF8.GetBytes("test");
            using (var ms = new MemoryStream(data))
            {
                request.Setup(x => x.Body).Returns(ms);
                context.Setup(x => x.Request).Returns(request.Object);
                context.Setup(x => x.Response).Returns(response.Object);

                await this.botMiddleware.Invoke(context.Object, this.connectorMock.Object);
            }
        }
    }
}
