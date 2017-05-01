using Qooba.Framework.Bot.Abstractions;
using System.Threading.Tasks;
using Qooba.Framework.Bot.Abstractions.Models;

namespace Qooba.Framework.Bot
{
    public class TextReplyBuilder : IReplyBuilder<TextReplyMessage>
    {
        public async Task<ReplyMessage> ExecuteAsync(IConversationContext context, TextReplyMessage reply)
        {
            return new ReplyMessage
            {
                Text = reply.Text
            };
        }
    }

    public class TextReplyMessage
    {
        public string Text { get; set; }
    }
}
