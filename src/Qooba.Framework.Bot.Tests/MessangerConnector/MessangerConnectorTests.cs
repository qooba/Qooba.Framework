using Moq;
using System.Net.Http;
using System.Threading.Tasks;
using System;
using Qooba.Framework.Bot.Abstractions;
using Qooba.Framework.Logging.Abstractions;
using Xunit;
using Qooba.Framework.Bot.Abstractions.Models;

namespace Qooba.Framework.Bot.Tests
{
    public class MessangerConnectorTests
    {
        private MessangerConnector messangerConnector;

        private Mock<IMessangerSecurity> messangerSecurityMock;

        private Mock<ITelemetry> telemetryMock;

        private Mock<ILogger> loggerMock;

        private Mock<IMessageQueue<string>> messageQueueMock;

        public MessangerConnectorTests()
        {
            this.messageQueueMock = new Mock<IMessageQueue<string>>();
            this.loggerMock = new Mock<ILogger>();
            this.telemetryMock = new Mock<ITelemetry>();
            this.telemetryMock.Setup(x => x.GlobalExceptionHandler(It.IsAny<Func<Task<HttpResponseMessage>>>(), It.IsAny<bool>())).Returns<Func<Task<HttpResponseMessage>>, bool>((x, b) => x());
            this.messangerSecurityMock = new Mock<IMessangerSecurity>();
            this.messangerConnector = new MessangerConnector(this.messangerSecurityMock.Object, this.telemetryMock.Object, this.loggerMock.Object, this.messageQueueMock.Object);
        }

        [Fact]
        public void ChallengerHandlingTest()
        {
            this.messangerSecurityMock.Setup(x => x.IsChallengeRequest(It.IsAny<HttpRequestMessage>())).Returns(new ChallengeResult(true, new HttpResponseMessage(System.Net.HttpStatusCode.OK)));

            var response = this.messangerConnector.Process(new HttpRequestMessage()).Result;

            Assert.True(response.StatusCode == System.Net.HttpStatusCode.OK);
            this.telemetryMock.Verify(x => x.TrackEvent("MessangerConnector-Challenge", "Challenge validation"), Times.Once);
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
            this.telemetryMock.Verify(x => x.TrackEvent("MessangerConnector-Signature", "Invalid signature"), Times.Once);
            this.telemetryMock.Verify(x => x.TrackEvent("MessangerConnector-Challenge", "Challenge validation"), Times.Never);
        }

        [Fact]
        public void ProcessMessageTest()
        {
            var json = "{\"object\":\"page\",\"entry\":[{\"id\":\"1\",\"time\":2,\"messaging\":[{\"sender\":{\"id\":\"3\"},\"recipient\":{\"id\":\"4\"},\"timestamp\":5,\"delivery\":{\"mids\":[\"mid.6:7\"],\"watermark\":8,\"seq\":0}}]}]}";
            this.messangerSecurityMock.Setup(x => x.IsChallengeRequest(It.IsAny<HttpRequestMessage>())).Returns(new ChallengeResult(false, null));
            this.messangerSecurityMock.Setup(x => x.ValidateSignature(It.IsAny<HttpRequestMessage>(), It.IsAny<string>())).Returns(true);
            var request = new HttpRequestMessage();
            request.Content = new StringContent(json);

            var response = this.messangerConnector.Process(request).Result;

            Assert.True(response.StatusCode == System.Net.HttpStatusCode.OK);
            this.telemetryMock.Verify(x => x.TrackEvent("MessangerConnector-Signature", "Invalid signature"), Times.Never);
            this.telemetryMock.Verify(x => x.TrackEvent("MessangerConnector-Challenge", "Challenge validation"), Times.Never);
            this.telemetryMock.Verify(x => x.TrackEvent("MessangerConnector-Request", json), Times.Once);
            this.messageQueueMock.Verify(x => x.Enqueue(It.IsAny<string>()), Times.Once);
        }
    }
}
