﻿using Qooba.Framework.Bot.Abstractions;
using System;
using System.Threading.Tasks;

namespace Qooba.Framework.Bot.Handlers
{
    public class DispatchHandler : BaseHandler, IHandler
    {
        private readonly Func<object, IDispatcher> replyClientFunc;

        public DispatchHandler(Func<object, IDispatcher> replyClientFunc)
        {
            this.replyClientFunc = replyClientFunc;
        }

        public override int Priority => 4;

        public override async Task InvokeAsync(IConversationContext conversationContext)
        {
            var replyClient = this.replyClientFunc(conversationContext.ConnectorType);

            if (conversationContext.Reply != null)
            {
                await replyClient.SendAsync(conversationContext.Reply);
            }

            await base.InvokeAsync(conversationContext);
        }
    }
}