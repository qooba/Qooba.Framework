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

        public async Task<Reply> BuildAsync(IConversationContext context, ReplyItem replyItem)
        {
            return new Reply
            {
                Recipient = new Recipient { Id = context?.Entry?.Message?.Sender?.Id },
                NotificationType = replyItem.NotificationType,
                SenderAction = replyItem.SenderAction,
                Message = await this.BuildAsync(context, replyItem.Reply as T)
            };
        }

        public abstract Task<ReplyMessage> BuildAsync(IConversationContext context, T reply);
    }
}
