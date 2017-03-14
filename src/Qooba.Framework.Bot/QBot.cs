using Newtonsoft.Json;
using Qooba.Framework.Bot.Abstractions;
using Qooba.Framework.Bot.Abstractions.Models;
using Qooba.Framework.Bot.Context;
using Qooba.Framework.Logging.Abstractions;
using System.Threading.Tasks;

namespace Qooba.Framework.Bot
{
    public class QBot : IBot
    {
        private readonly ILogger logger;

        private readonly IHandlerManager handlerManager;

        public QBot(ILogger logger, IHandlerManager handlerManager)
        {
            this.logger = logger;
            this.handlerManager = handlerManager;
        }

        public async Task Run(string queueItem)
        {
            var entry = JsonConvert.DeserializeObject<Entry>(queueItem, Serialization.Settings);
            this.logger.Info($"Bot-StartProcess: {queueItem}");
            IConversationContext context = new ConversationContext
            {
                Entry = entry
            };

            var handler = await this.handlerManager.CreateAsync(context);
            await handler.InvokeAsync(context);
        }
    }
}
