using Qooba.Framework.Bot.Abstractions;
using System.Threading.Tasks;
using Qooba.Framework.Bot.Abstractions.Models;
using System.Collections.Generic;
using System.Linq;

namespace Qooba.Framework.Bot
{
    public class EnumReplyBuilder : IReplyBuilder<EnumReplyMessage>
    {
        public async Task<ReplyMessage> ExecuteAsync(IConversationContext context, EnumReplyMessage reply)
        {
            return new ReplyMessage
            {
                Text = reply.Text,
                Quick_replies = reply.Enum.Select(x => new QuickReply { Content_type = ContentType.text, Title = x.Title, Payload = x.Payload ?? x.Title }).ToList()
            };
        }
    }

    public class EnumReplyMessage
    {
        public string Text { get; set; }

        public IEnumerable<EnumReplyItem> Enum { get; set; }
    }

    public class EnumReplyItem
    {
        public string Title { get; set; }

        public string Payload { get; set; }
    }
}
