using Qooba.Framework.Bot.Abstractions;
using System.Threading.Tasks;
using Qooba.Framework.Bot.Abstractions.Models;
using Qooba.Framework.Bot.Abstractions.Models.Attachments;

namespace Qooba.Framework.Bot
{
    public class AudioReplyBuilder : IReplyBuilder<AudioReplyMessage>
    {
        public async Task<ReplyMessage> ExecuteAsync(IConversationContext context, AudioReplyMessage reply)
        {
            return new ReplyMessage
            {
                Attachment = new AudioAttachment
                {
                    Payload = new MediaAttachmentPayload
                    {
                        Url = reply.Audio
                    }
                }
            };
        }
    }

    public class AudioReplyMessage
    {
        public string Audio { get; set; }
    }
}
