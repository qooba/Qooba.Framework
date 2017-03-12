using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Net.Http;

namespace AbacoosBotFunc.Tests
{
    [TestClass]
    public class MessangerSecurityTests
    {
        private IMessangerSecurity messangerSecurity;

        private Mock<IConfig> configMock;

        [TestInitialize]
        public void Initialize()
        {
            this.configMock = new Mock<IConfig>();
            this.configMock.Setup(x => x.Get(Constants.MessangerAppSecret)).Returns("1111");
            this.configMock.Setup(x => x.Get(Constants.MessangerChallengeVerifyToken)).Returns("54321");
            this.messangerSecurity = new MessangerSecurity(this.configMock.Object);
        }

        [TestMethod]
        public void ValidateSignatureSuccessTest()
        {
            var request = new HttpRequestMessage();
            request.Headers.Add("X-Hub-Signature", "sha1=9fe7e215e8ff82c8ed65d3acb1459c3df28df11e");
            var content = "{\"object\":\"page\",\"entry\":[{\"id\":\"1864721730470949\",\"time\":1488979817970,\"messaging\":[{\"sender\":{\"id\":\"1417157201690099\"},\"recipient\":{\"id\":\"1864721730470949\"},\"timestamp\":1488979817969,\"delivery\":{\"mids\":[\"mid.1488979817438:29d60d8260\"],\"watermark\":1488979817438,\"seq\":0}}]}]}";

            var isValid = this.messangerSecurity.ValidateSignature(request, content);

            Assert.IsTrue(isValid);
        }

        [TestMethod]
        public void ValidateSignatureFalseTest()
        {
            var request = new HttpRequestMessage();
            request.Headers.Add("X-Hub-Signature", "sha1=bf758c72196d453c059ea8abf3389d99d19acb92");
            var content = "{\"object\":\"page\",\"entry\":[{\"id\":\"1864721730470949\",\"time\":1488979817970,\"messaging\":[{\"sender\":{\"id\":\"1417157201690099\"},\"recipient\":{\"id\":\"1864721730470949\"},\"timestamp\":1488979817969,\"delivery\":{\"mids\":[\"mid.1488979817438:29d60d8260\"],\"watermark\":1488979817438,\"seq\":0}}]}]}";

            var isValid = this.messangerSecurity.ValidateSignature(request, content);

            Assert.IsFalse(isValid);
        }

        [TestMethod]
        public void IsChallengeRequestTrueAndSuccessTest()
        {
            var challenge = "1234";
            var verifyToken = "54321";
            var mode = "subscribe";
            var request = PrepareRequest(challenge, verifyToken, mode);

            var result = this.messangerSecurity.IsChallengeRequest(request);

            Assert.IsTrue(result.IsChallenge);
            Assert.IsTrue(result.Response.Content.ReadAsStringAsync().Result == challenge);
        }
        
        [TestMethod]
        public void IsChallengeRequestTrueAndFailedTest()
        {
            var challenge = "1234";
            var verifyToken = "12345";
            var mode = "subscribe";
            var request = PrepareRequest(challenge, verifyToken, mode);

            var result = this.messangerSecurity.IsChallengeRequest(request);

            Assert.IsTrue(result.IsChallenge);
            Assert.IsFalse(result.Response.Content.ReadAsStringAsync().Result == challenge);
            Assert.IsTrue(result.Response.StatusCode == System.Net.HttpStatusCode.BadRequest);
        }

        [TestMethod]
        public void IsChallengeRequestFalseTest()
        {
            var challenge = "1234";
            var verifyToken = "54321";
            var mode = "test";
            var request = PrepareRequest(challenge, verifyToken, mode);

            var result = this.messangerSecurity.IsChallengeRequest(request);

            Assert.IsFalse(result.IsChallenge);
            Assert.IsNull(result.Response);
        }

        private static HttpRequestMessage PrepareRequest(string challenge, string verifyToken, string mode)
        {
            var request = new HttpRequestMessage();
            request.RequestUri = new Uri($"http://test.pl?hub.mode={mode}&hub.challenge={challenge}&hub.verify_token={verifyToken}");
            return request;
        }
    }
}
