using Qooba.Framework.Bot.Abstractions;
using System;
using System.Threading.Tasks;
using Qooba.Framework.Bot.Abstractions.Models;

namespace Qooba.Framework.Bot
{
    public abstract class BaseReplyBuilder<T> : IReplyBuilder<T>
        where T : class
    {
        public Type ReplyItemType => typeof(T);

        public Task<ReplyMessage> BuildAsync(IConversationContext context, object reply) => this.BuildAsync(context, reply as T);

        public abstract Task<ReplyMessage> BuildAsync(IConversationContext context, T reply);
    }
}
