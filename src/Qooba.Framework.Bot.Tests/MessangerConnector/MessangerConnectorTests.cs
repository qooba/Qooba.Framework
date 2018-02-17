using Moq;
using System.Net.Http;
using Qooba.Framework.Bot.Abstractions;
using Qooba.Framework.Abstractions;
using Xunit;
using Qooba.Framework.Bot.Abstractions.Models;

namespace Qooba.Framework.Bot.Tests
{
    public class MessangerConnectorTests
    {
        private MessangerConnector messangerConnector;

        private Mock<IMessangerSecurity> messangerSecurityMock;

        private Mock<ILogger> loggerMock;

        private Mock<IMessageQueue> messageQueueMock;

        public MessangerConnectorTests()
        {
            this.messageQueueMock = new Mock<IMessageQueue>();
            this.loggerMock = new Mock<ILogger>();
            this.messangerSecurityMock = new Mock<IMessangerSecurity>();
            this.messangerConnector = new MessangerConnector(this.messangerSecurityMock.Object, this.loggerMock.Object, this.messageQueueMock.Object);
        }

        [Fact]
        public void ChallengerHandlingTest()
        {
            this.messangerSecurityMock.Setup(x => x.IsChallengeRequest(It.IsAny<HttpRequestMessage>())).Returns(new ChallengeResult(true, new HttpResponseMessage(System.Net.HttpStatusCode.OK)));

            var response = this.messangerConnector.Process(new HttpRequestMessage()).Result;

            Assert.True(response.StatusCode == System.Net.HttpStatusCode.OK);
            this.loggerMock.Verify(x => x.Info("Challenge validation"), Times.Once);
        }

        [Fact]
        public void InvalidSignatureTest()
        {
            this.messangerSecurityMock.Setup(x => x.IsChallengeRequest(It.IsAny<HttpRequestMessage>())).Returns(new ChallengeResult(false, null));
            this.messangerSecurityMock.Setup(x => x.ValidateSignature(It.IsAny<HttpRequestMessage>(), It.IsAny<string>())).Returns(false);
            var request = new HttpRequestMessage();
            request.Content = new StringContent("{\"object\":\"page\",\"entry\":[{\"id\":\"1\",\"time\":2,\"messaging\":[{\"sender\":{\"id\":\"3\"},\"recipient\":{\"id\":\"4\"},\"timestamp\":5,\"delivery\":{\"mids\":[\"mid.5:7\"],\"watermark\":8,\"seq\":0}}]}]}");

            var response = this.messangerConnector.Process(request).Result;

            Assert.True(response.StatusCode == System.Net.HttpStatusCode.OK);
            this.loggerMock.Verify(x => x.Info("Invalid signature"), Times.Once);
            this.loggerMock.Verify(x => x.Info("Challenge validation"), Times.Never);
        }

        [Fact]
        public void ProcessMessageTest()
        {
            var json = "{\"object\":\"page\",\"entry\":[{\"id\":\"1\",\"time\":2,\"messaging\":[{\"sender\": {\"id\": \"1417157201690099\"},\"recipient\": {\"id\": \"1864721730470949\"},\"timestamp\": 1489246577601,\"message\": {\"mid\": \"mid.1489246577601:900ac22e12\",\"seq\": 3850,\"text\": \"Allo\"}}]}]}";
            this.messangerSecurityMock.Setup(x => x.IsChallengeRequest(It.IsAny<HttpRequestMessage>())).Returns(new ChallengeResult(false, null));
            this.messangerSecurityMock.Setup(x => x.ValidateSignature(It.IsAny<HttpRequestMessage>(), It.IsAny<string>())).Returns(true);
            var request = new HttpRequestMessage();
            request.Content = new StringContent(json);

            var response = this.messangerConnector.Process(request).Result;

            Assert.True(response.StatusCode == System.Net.HttpStatusCode.OK);
            this.loggerMock.Verify(x => x.Info("Invalid signature"), Times.Never);
            this.loggerMock.Verify(x => x.Info("Challenge validation"), Times.Never);
            this.loggerMock.Verify(x => x.Info(json), Times.Once);
            this.messageQueueMock.Verify(x => x.EnqueueAsync(It.IsAny<string>()), Times.Once);
        }
    }
}
