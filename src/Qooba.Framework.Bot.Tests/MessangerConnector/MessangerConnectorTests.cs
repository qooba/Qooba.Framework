using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Net.Http;
using Microsoft.Azure.WebJobs;
using System.Threading.Tasks;
using System;

namespace AbacoosBotFunc.Tests
{
    [TestClass]
    public class MessangerConnectorTests
    {
        private MessangerConnector messangerConnector;

        private Mock<IMessangerSecurity> messangerSecurityMock;

        private Mock<ITelemetry> telemetryMock;

        private Mock<ILogger> loggerMock;

        private Mock<ICollector<string>> collectorMock;

        [TestInitialize]
        public void Initialize()
        {
            this.collectorMock = new Mock<ICollector<string>>();
            this.loggerMock = new Mock<ILogger>();
            this.telemetryMock = new Mock<ITelemetry>();
            this.telemetryMock.Setup(x => x.GlobalExceptionHandler(It.IsAny<Func<Task<HttpResponseMessage>>>(), It.IsAny<bool>())).Returns<Func<Task<HttpResponseMessage>>, bool>((x, b) => x());
            this.messangerSecurityMock = new Mock<IMessangerSecurity>();
            this.messangerConnector = new MessangerConnector(this.messangerSecurityMock.Object, this.telemetryMock.Object, this.loggerMock.Object);
        }

        [TestMethod]
        public void ChallengerHandlingTest()
        {
            this.messangerSecurityMock.Setup(x => x.IsChallengeRequest(It.IsAny<HttpRequestMessage>())).Returns(new ChallengeResult(true, new HttpResponseMessage(System.Net.HttpStatusCode.OK)));

            var response = this.messangerConnector.Run(new HttpRequestMessage(), this.collectorMock.Object).Result;

            Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.OK);
            this.telemetryMock.Verify(x => x.TrackEvent("MessangerConnector-Challenge", "Challenge validation"), Times.Once);
        }

        [TestMethod]
        public void InvalidSignatureTest()
        {
            this.messangerSecurityMock.Setup(x => x.IsChallengeRequest(It.IsAny<HttpRequestMessage>())).Returns(new ChallengeResult(false, null));
            this.messangerSecurityMock.Setup(x => x.ValidateSignature(It.IsAny<HttpRequestMessage>(), It.IsAny<string>())).Returns(false);
            var request = new HttpRequestMessage();
            request.Content = new StringContent("{\"object\":\"page\",\"entry\":[{\"id\":\"1\",\"time\":2,\"messaging\":[{\"sender\":{\"id\":\"3\"},\"recipient\":{\"id\":\"4\"},\"timestamp\":5,\"delivery\":{\"mids\":[\"mid.5:7\"],\"watermark\":8,\"seq\":0}}]}]}");

            var response = this.messangerConnector.Run(request, this.collectorMock.Object).Result;

            Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.OK);
            this.telemetryMock.Verify(x => x.TrackEvent("MessangerConnector-Signature", "Invalid signature"), Times.Once);
            this.telemetryMock.Verify(x => x.TrackEvent("MessangerConnector-Challenge", "Challenge validation"), Times.Never);
        }

        [TestMethod]
        public void ProcessMessageTest()
        {
            var json = "{\"object\":\"page\",\"entry\":[{\"id\":\"1\",\"time\":2,\"messaging\":[{\"sender\":{\"id\":\"3\"},\"recipient\":{\"id\":\"4\"},\"timestamp\":5,\"delivery\":{\"mids\":[\"mid.6:7\"],\"watermark\":8,\"seq\":0}}]}]}";
            this.messangerSecurityMock.Setup(x => x.IsChallengeRequest(It.IsAny<HttpRequestMessage>())).Returns(new ChallengeResult(false, null));
            this.messangerSecurityMock.Setup(x => x.ValidateSignature(It.IsAny<HttpRequestMessage>(), It.IsAny<string>())).Returns(true);
            var request = new HttpRequestMessage();
            request.Content = new StringContent(json);

            var response = this.messangerConnector.Run(request, this.collectorMock.Object).Result;

            Assert.IsTrue(response.StatusCode == System.Net.HttpStatusCode.OK);
            this.telemetryMock.Verify(x => x.TrackEvent("MessangerConnector-Signature", "Invalid signature"), Times.Never);
            this.telemetryMock.Verify(x => x.TrackEvent("MessangerConnector-Challenge", "Challenge validation"), Times.Never);
            this.telemetryMock.Verify(x => x.TrackEvent("MessangerConnector-Request", json), Times.Once);
            this.collectorMock.Verify(x => x.Add(It.IsAny<string>()), Times.Once);
        }
    }
}
