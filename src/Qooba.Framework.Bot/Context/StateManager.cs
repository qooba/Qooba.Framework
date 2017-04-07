﻿using Qooba.Framework.Bot.Abstractions;
using Qooba.Framework.Configuration.Abstractions;
using System.Threading.Tasks;

namespace Qooba.Framework.Bot.Context
{
    public class StateManager : IStateManager
    {
        private readonly IBotConfig config;

        public StateManager(IBotConfig config)
        {
            this.config = config;
        }

        public async Task<IConversationContext> FetchContext(IConversationContext context)
        {
            return context;
        }

        public async Task SaveContext(IConversationContext context)
        {

        }
    }
}