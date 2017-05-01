using Qooba.Framework.Bot.Abstractions;
using System.Threading.Tasks;
using Qooba.Framework.Bot.Abstractions.Models;

namespace Qooba.Framework.Bot
{
    public class LocationReplyBuilder : IReplyBuilder<LocationReplyMessage>
    {
        public async Task<ReplyMessage> ExecuteAsync(IConversationContext context, LocationReplyMessage reply)
        {
            return new ReplyMessage
            {
                Text = reply.Text,
                Quick_replies = new[] { new QuickReply { Content_type = ContentType.location } }
            };
        }
    }

    public class LocationReplyMessage
    {
        public string Text { get; set; }
    }
}
