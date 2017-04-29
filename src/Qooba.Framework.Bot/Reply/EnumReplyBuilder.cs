using Qooba.Framework.Bot.Abstractions;
using System.Threading.Tasks;
using Qooba.Framework.Bot.Abstractions.Models;
using System.Collections.Generic;
using System.Linq;

namespace Qooba.Framework.Bot
{
    public class EnumReplyBuilder : IReplyBuilder<EnumReplyMessage>
    {
        public async Task<ReplyMessage> BuildAsync(IConversationContext context, EnumReplyMessage reply)
        {
            return new ReplyMessage
            {
                Text = reply.Text,
                Quick_replies = reply.Enum.Select(x => new QuickReply { Content_type = ContentType.text, Title = x, Payload = x }).ToList()
            };
        }
    }

    public class EnumReplyMessage
    {
        public string Text { get; set; }

        public IEnumerable<string> Enum { get; set; }
    }
}
