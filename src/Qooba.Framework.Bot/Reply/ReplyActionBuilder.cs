using System.Threading.Tasks;
using Qooba.Framework.Bot.Abstractions;
using Qooba.Framework.Bot.Abstractions.Models;
using System.Collections.Generic;

namespace Qooba.Framework.Bot
{
    public class ReplyActionBuilder<TReplyMessage, TReplyAction> : IReplyBuilder<ReplyActionMessage>
        where TReplyMessage : class
        where TReplyAction : class, IReplyAction<TReplyMessage>
    {
        private readonly IReplyBuilder<TReplyMessage> replyBuilder;

        private readonly TReplyAction replyAction;

        public ReplyActionBuilder(IReplyBuilder<TReplyMessage> replyBuilder, TReplyAction replyAction)
        {
            this.replyBuilder = replyBuilder;
            this.replyAction = replyAction;
        }

        public async Task<ReplyMessage> ExecuteAsync(IConversationContext context, ReplyActionMessage reply)
        {
            var message = await this.replyAction.CreateReplyMessage(context, reply?.Parameters);
            return await this.replyBuilder.ExecuteAsync(context, message);
        }
    }

    public class ReplyActionMessage
    {
        public IDictionary<string, string> Parameters { get; set; }
    }
}
