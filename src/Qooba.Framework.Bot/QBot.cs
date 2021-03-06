﻿using Qooba.Framework.Bot.Abstractions;
using Qooba.Framework.Bot.Abstractions.Models;
using Qooba.Framework.Abstractions;
using System.Threading.Tasks;

namespace Qooba.Framework.Bot
{
    public class QBot : IBot
    {
        private readonly ILogger logger;

        private readonly IHandlerManager handlerManager;

        private readonly ISerializer serializer;

        public QBot(ILogger logger, IHandlerManager handlerManager, ISerializer serializer)
        {
            this.logger = logger;
            this.handlerManager = handlerManager;
            this.serializer = serializer;
        }

        public async Task Run(string queueItem)
        {
            var entry = this.serializer.Deserialize<Entry>(queueItem);
            this.logger.Info($"Bot-StartProcess: {queueItem}");
            await this.Run(entry);
        }

        public async Task Run(Entry entry)
        {
            IConversationContext context = new ConversationContext
            {
                Entry = entry
            };

            var handler = await this.handlerManager.CreateAsync(context);
            await handler.InvokeAsync(context);
        }

        public async Task Redirect(string routeText, IConversationContext conversationContext)
        {
            conversationContext.Entry.Message.Message.Text = routeText;
            conversationContext.Entry.Message.Message.Quick_reply = null;
            var handler = await this.handlerManager.CreateAsync(conversationContext);
            await handler.InvokeAsync(conversationContext);
        }
    }
}
