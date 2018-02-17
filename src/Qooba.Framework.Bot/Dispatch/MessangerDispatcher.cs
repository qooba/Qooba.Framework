using Newtonsoft.Json;
using Qooba.Framework.Bot.Abstractions;
using Qooba.Framework.Bot.Abstractions.Models;
using Qooba.Framework.Abstractions;
using Qooba.Framework.Serialization.Abstractions;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Qooba.Framework.Bot.Dispatch
{
    public class MessangerDispatcher : IDispatcher
    {
        private readonly ILogger logger;

        private readonly IBotConfig config;

        private readonly ISerializer serializer;

        public MessangerDispatcher(ILogger logger, IBotConfig config, ISerializer serializer)
        {
            this.logger = logger;
            this.config = config;
            this.serializer = serializer;
        }

        public ConnectorType ConnectorType => ConnectorType.Messanger;

        public async Task SendAsync(Reply reply)
        {
            using (var client = new HttpClient())
            {
                var accessToken = this.config.MessangerAccessToken;
                var request = this.serializer.Serialize(reply);
                var content = new StringContent(request, Encoding.UTF8, "application/json");
                var response = await client.PostAsync($"https://graph.facebook.com/v2.6/me/messages?access_token={accessToken}", content);
                var responseString = await response.Content.ReadAsStringAsync();
                this.logger.Info(responseString);
            }
        }
    }
}
