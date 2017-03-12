using Qooba.Framework.Bot.Abstractions.Models;
using System.Net.Http;

namespace Qooba.Framework.Bot.Abstractions
{
    public interface IMessangerSecurity
    {
        ChallengeResult IsChallengeRequest(HttpRequestMessage request);

        bool ValidateSignature(HttpRequestMessage request, string content);
    }
}