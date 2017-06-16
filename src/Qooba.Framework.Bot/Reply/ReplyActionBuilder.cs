using System.Threading.Tasks;
using Qooba.Framework.Bot.Abstractions;
using Qooba.Framework.Bot.Abstractions.Models;
using System;
using Qooba.Framework.Abstractions;

namespace Qooba.Framework.Bot.Reply
{
    public class ReplyActionBuilder<T> : IReplyBuilder<ReplyActionMessage>
        where T : class
    {
        private readonly IReplyBuilder<T> replyBuilder;

        public ReplyActionBuilder(IReplyBuilder<T> replyBuilder)
        {
            this.replyBuilder = replyBuilder;
        }

        public Func<IReplyAction<T>> Action { get; set; }

        public async Task<ReplyMessage> ExecuteAsync(IConversationContext context, ReplyActionMessage reply)
        {
            var message = await this.Action().CreateReplyMessage(context);
            return await this.replyBuilder.ExecuteAsync(context, message);
        }
    }

    public class ReplyActionMessage
    {

    }
}
