using System.Net;
using System.Threading.Tasks;
using System.Net.Http;
using Qooba.Framework.Bot.Abstractions;
using Qooba.Framework.Logging.Abstractions;
using Newtonsoft.Json.Linq;
using Qooba.Framework.Bot.Abstractions.Models;

namespace Qooba.Framework.Bot
{
    public class MessangerConnector
    {
        private readonly IMessangerSecurity messangerSecurity;

        private readonly ITelemetry telemetry;

        private readonly ILogger logger;

        public MessangerConnector(IMessangerSecurity messangerSecurity, ITelemetry telemetry, ILogger logger)
        {
            this.logger = logger;
            this.messangerSecurity = messangerSecurity;
            this.telemetry = telemetry;
        }
        
        private async Task<HttpResponseMessage> Process(HttpRequestMessage req, ICollector<string> myQueue)
        {
            var challengeResult = this.messangerSecurity.IsChallengeRequest(req);
            if (challengeResult.IsChallenge)
            {
                this.telemetry.TrackEvent("MessangerConnector-Challenge", "Challenge validation");
                return challengeResult.Response;
            }

            var json = await req.Content.ReadAsStringAsync();
            if (!this.messangerSecurity.ValidateSignature(req, json))
            {
                this.telemetry.TrackEvent("MessangerConnector-Signature", "Invalid signature");
                return new HttpResponseMessage(HttpStatusCode.OK);
            }

            this.telemetry.TrackEvent("MessangerConnector-Request", json);
            var o = JObject.Parse(json);
            var entries = (JArray)o["entry"];
            foreach (var entry in entries)
            {
                var messaging = entry["messaging"];
                foreach (var message in messaging)
                {
                    var m = new JObject(new JProperty("connectorType", ConnectorType.Messanger.ToString()), new JProperty("message", message));
                    myQueue.Add(m.ToString());
                }
            }

            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}