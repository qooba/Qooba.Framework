using System.Net.Http;

namespace Qooba.Framework.Bot.Abstractions.Models
{
    public class ChallengeResult
    {
        public ChallengeResult(bool isChallenge, HttpResponseMessage response)
        {
            this.IsChallenge = isChallenge;
            this.Response = response;
        }

        public bool IsChallenge { get; set; }

        public HttpResponseMessage Response { get; set; }
    }
}
