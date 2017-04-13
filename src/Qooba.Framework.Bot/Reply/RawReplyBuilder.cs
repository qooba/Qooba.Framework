using Qooba.Framework.Bot.Abstractions;
using System.Threading.Tasks;
using Qooba.Framework.Bot.Abstractions.Models;

namespace Qooba.Framework.Bot
{
    public class RawReplyBuilder : BaseReplyBuilder<ReplyMessage>
    {
        public override Task<ReplyMessage> BuildAsync(IConversationContext context, ReplyMessage reply) => Task.FromResult(reply);
    }
}
