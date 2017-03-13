using System.Net;
using System.Threading.Tasks;
using System.Net.Http;
using Qooba.Framework.Bot.Abstractions;
using Qooba.Framework.Logging.Abstractions;
using Newtonsoft.Json.Linq;
using Qooba.Framework.Bot.Abstractions.Models;

namespace Qooba.Framework.Bot
{
    public class MessangerConnector : IConnector
    {
        private readonly IMessangerSecurity messangerSecurity;

        private readonly ILogger logger;

        private readonly IMessageQueue<string> queue;

        public MessangerConnector(IMessangerSecurity messangerSecurity, ILogger logger, IMessageQueue<string> queue)
        {
            this.logger = logger;
            this.messangerSecurity = messangerSecurity;
            this.queue = queue;
        }

        public async Task<HttpResponseMessage> Process(HttpRequestMessage req)
        {
            var challengeResult = this.messangerSecurity.IsChallengeRequest(req);
            if (challengeResult.IsChallenge)
            {
                this.logger.Info("Challenge validation");
                return challengeResult.Response;
            }

            var json = await req.Content.ReadAsStringAsync();
            if (!this.messangerSecurity.ValidateSignature(req, json))
            {
                this.logger.Info("Invalid signature");
                return new HttpResponseMessage(HttpStatusCode.OK);
            }

            this.logger.Info(json);
            var o = JObject.Parse(json);
            var entries = (JArray)o["entry"];
            foreach (var entry in entries)
            {
                var messaging = entry["messaging"];
                foreach (var message in messaging)
                {
                    var m = new JObject(new JProperty("connectorType", ConnectorType.Messanger.ToString()), new JProperty("message", message));
                    await this.queue.Enqueue(m.ToString());
                }
            }

            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}