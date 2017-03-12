using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Qooba.Framework.Bot.Abstractions;
using Qooba.Framework.Bot.Abstractions.Models;
using Qooba.Framework.Configuration.Abstractions;
using Qooba.Framework.Logging.Abstractions;
using System;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;

namespace Qooba.Framework.Bot
{
    public class MessangerDispatcher : IDispatcher
    {
        private readonly ILogger logger;

        private readonly ITelemetry telemetry;

        private readonly IConfig config;

        private static Lazy<JsonMediaTypeFormatter> formatter = new Lazy<JsonMediaTypeFormatter>(() =>
        {
            var formatter = new JsonMediaTypeFormatter
            {
                SerializerSettings = Serialization.Settings
            };
            return formatter;
        });

        public MessangerDispatcher(ITelemetry telemetry, ILogger logger, IConfig config)
        {
            this.telemetry = telemetry;
            this.logger = logger;
            this.config = config;
        }

        public ConnectorType ConnectorType => ConnectorType.Messanger;

        public async Task SendAsync(Reply reply)
        {
            using (var client = new HttpClient())
            {
                if (reply == null)
                {
                    return;
                }

                var accessToken = this.config[Constants.MessangerAccessToken];
                var response = await client.PostAsync($"https://graph.facebook.com/v2.6/me/messages?access_token={accessToken}", reply, formatter.Value);
                var responseString = await response.Content.ReadAsStringAsync();
                this.logger.Info(responseString);
            }
        }
    }
}
