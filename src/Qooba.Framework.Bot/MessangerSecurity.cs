using Microsoft.AspNetCore.WebUtilities;
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
        private readonly IConfig config;

        public MessangerSecurity(IConfig config)
        {
            this.config = config;
        }

        public ChallengeResult IsChallengeRequest(HttpRequestMessage request)
        {
            var queries = QueryHelpers.ParseQuery(request.RequestUri.Query);
            var hubMode = queries["hub.mode"];
            var hubChallenge = queries["hub.challenge"];
            var hubVerifyToken = queries["hub.verify_token"];
            var messangerChallengeVerifyToken = this.config[Constants.MessangerChallengeVerifyToken];

            if (hubMode == "subscribe")
            {
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
            var messangerAppSecret = this.config[Constants.MessangerAppSecret];
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