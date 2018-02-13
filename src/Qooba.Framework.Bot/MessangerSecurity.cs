using Qooba.Framework.Bot.Abstractions;
using Qooba.Framework.Bot.Abstractions.Models;
using Qooba.Framework.Configuration.Abstractions;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;

namespace Qooba.Framework.Bot
{
    public class MessangerSecurity : IMessangerSecurity
    {
        private readonly IBotConfig config;

        public MessangerSecurity(IBotConfig config)
        {
            this.config = config;
        }

        public ChallengeResult IsChallengeRequest(HttpRequestMessage request)
        {
            var queries = request.RequestUri.Query.Replace("?", "").Split('&').ToDictionary(x => x.Split('=')[0], x => x.Split('=')[1]);;
            queries.TryGetValue("hub.mode", out var hubMode);

            if (hubMode == "subscribe")
            {
                var hubChallenge = queries["hub.challenge"];
                var hubVerifyToken = queries["hub.verify_token"];
                var messangerChallengeVerifyToken = this.config.MessangerChallengeVerifyToken;

                if (hubVerifyToken == messangerChallengeVerifyToken)
                {
                    var response = PrepareResponse(HttpStatusCode.OK, hubChallenge);
                    return new ChallengeResult(true, response);
                }
                else
                {
                    var response = PrepareResponse(HttpStatusCode.BadRequest, string.Empty);
                    return new ChallengeResult(true, response);
                }
            }

            return new ChallengeResult(false, null);
        }

        public bool ValidateSignature(HttpRequestMessage request, string content)
        {
            var signature = request.Headers.GetValues("X-Hub-Signature").FirstOrDefault().Substring(5);
            var messangerAppSecret = this.config.MessangerAppSecret;
            var hash = Encode(content, messangerAppSecret);
            return signature == hash;
        }

        private static string Encode(string input, string key)
        {
            var encoding = Encoding.UTF8;
            var k = encoding.GetBytes(key);
            var i = encoding.GetBytes(input);

            using (var sha = new HMACSHA1(k))
            using (var ms = new MemoryStream(i))
            {
                return sha.ComputeHash(ms).Aggregate(string.Empty, (s, e) => string.Concat(s, string.Format("{0:x2}", e)));
            }
        }

        private static HttpResponseMessage PrepareResponse(HttpStatusCode status, string content)
        {
            var response = new HttpResponseMessage(status);
            response.Content = new StringContent(content, Encoding.UTF8, "text/plain");
            return response;
        }
    }
}