using Qooba.Framework.Bot.Abstractions;
using System.Threading.Tasks;
using Qooba.Framework.Bot.Abstractions.Models;

namespace Qooba.Framework.Bot
{
    public class RawReplyBuilder : IReplyBuilder<RawReplyMessage>
    {
        public Task<ReplyMessage> ExecuteAsync(IConversationContext context, RawReplyMessage reply) => Task.FromResult((ReplyMessage)reply);
    }

    public class RawReplyMessage : ReplyMessage
    {
    }
}
