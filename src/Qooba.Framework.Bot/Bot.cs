using Newtonsoft.Json;
using Qooba.Framework.Bot.Abstractions;
using Qooba.Framework.Bot.Abstractions.Models;
using Qooba.Framework.Bot.Context;
using Qooba.Framework.Logging.Abstractions;
using System.Threading.Tasks;

namespace Qooba.Framework.Bot
{
    public class Bot
    {
        private readonly ITelemetry telemetry;

        private readonly ILogger logger;

        private readonly IHandler handler;

        public Bot(ITelemetry telemetry, ILogger logger, IHandler handler)
        {
            this.telemetry = telemetry;
            this.logger = logger;
            this.handler = handler;
        }

        public async Task Run(string myQueueItem)
        {
            var entry = JsonConvert.DeserializeObject<Entry>(myQueueItem, Serialization.Settings);
            this.telemetry.TrackEvent("Bot-StartProcess", myQueueItem);
            this.logger.Info($"C# Queue trigger function processed: {myQueueItem}");
            IConversationContext context = new ConversationContext
            {
                Entry = entry
            };

            await this.handler.InvokeAsync(context);
        }
    }
}
