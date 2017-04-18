using Qooba.Framework.Bot.Abstractions;
using System.Threading.Tasks;
using Qooba.Framework.Bot.Abstractions.Models;

namespace Qooba.Framework.Bot
{
    public class RawReplyBuilder : IReplyBuilder<ReplyMessage>
    {
        public Task<ReplyMessage> BuildAsync(IConversationContext context, ReplyMessage reply) => Task.FromResult(reply);
    }
}
