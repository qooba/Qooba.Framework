using System;
using Moq;
using System.Net.Http;
using Qooba.Framework.Bot.Abstractions;
using Qooba.Framework.Configuration.Abstractions;
using Xunit;

namespace Qooba.Framework.Bot.Tests
{
    public class MessangerSecurityTests
    {
        private IMessangerSecurity messangerSecurity;

        private Mock<IBotConfig> configMock;
        
        public MessangerSecurityTests()
        {
            this.configMock = new Mock<IBotConfig>();
            this.configMock.Setup(x => x.MessangerAppSecret).Returns("1111");
            this.configMock.Setup(x => x.MessangerChallengeVerifyToken).Returns("54321");
            this.messangerSecurity = new MessangerSecurity(this.configMock.Object);
        }

        [Fact]
        public void ValidateSignatureSuccessTest()
        {
            var request = new HttpRequestMessage();
            request.Headers.Add("X-Hub-Signature", "sha1=9fe7e215e8ff82c8ed65d3acb1459c3df28df11e");
            var content = "{\"object\":\"page\",\"entry\":[{\"id\":\"1864721730470949\",\"time\":1488979817970,\"messaging\":[{\"sender\":{\"id\":\"1417157201690099\"},\"recipient\":{\"id\":\"1864721730470949\"},\"timestamp\":1488979817969,\"delivery\":{\"mids\":[\"mid.1488979817438:29d60d8260\"],\"watermark\":1488979817438,\"seq\":0}}]}]}";

            var isValid = this.messangerSecurity.ValidateSignature(request, content);

            Assert.True(isValid);
        }

        [Fact]
        public void ValidateSignatureFalseTest()
        {
            var request = new HttpRequestMessage();
            request.Headers.Add("X-Hub-Signature", "sha1=bf758c72196d453c059ea8abf3389d99d19acb92");
            var content = "{\"object\":\"page\",\"entry\":[{\"id\":\"1864721730470949\",\"time\":1488979817970,\"messaging\":[{\"sender\":{\"id\":\"1417157201690099\"},\"recipient\":{\"id\":\"1864721730470949\"},\"timestamp\":1488979817969,\"delivery\":{\"mids\":[\"mid.1488979817438:29d60d8260\"],\"watermark\":1488979817438,\"seq\":0}}]}]}";

            var isValid = this.messangerSecurity.ValidateSignature(request, content);

            Assert.False(isValid);
        }

        [Fact]
        public void IsChallengeRequestTrueAndSuccessTest()
        {
            var challenge = "1234";
            var verifyToken = "54321";
            var mode = "subscribe";
            var request = PrepareRequest(challenge, verifyToken, mode);

            var result = this.messangerSecurity.IsChallengeRequest(request);

            Assert.True(result.IsChallenge);
            Assert.True(result.Response.Content.ReadAsStringAsync().Result == challenge);
        }
        
        [Fact]
        public void IsChallengeRequestTrueAndFailedTest()
        {
            var challenge = "1234";
            var verifyToken = "12345";
            var mode = "subscribe";
            var request = PrepareRequest(challenge, verifyToken, mode);

            var result = this.messangerSecurity.IsChallengeRequest(request);

            Assert.True(result.IsChallenge);
            Assert.False(result.Response.Content.ReadAsStringAsync().Result == challenge);
            Assert.True(result.Response.StatusCode == System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public void IsChallengeRequestFalseTest()
        {
            var challenge = "1234";
            var verifyToken = "54321";
            var mode = "test";
            var request = PrepareRequest(challenge, verifyToken, mode);

            var result = this.messangerSecurity.IsChallengeRequest(request);

            Assert.False(result.IsChallenge);
            Assert.Null(result.Response);
        }

        private static HttpRequestMessage PrepareRequest(string challenge, string verifyToken, string mode)
        {
            var request = new HttpRequestMessage();
            request.RequestUri = new Uri($"http://test.pl?hub.mode={mode}&hub.challenge={challenge}&hub.verify_token={verifyToken}");
            return request;
        }
    }
}
