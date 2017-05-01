using Qooba.Framework.Bot.Abstractions;
using System.Threading.Tasks;
using Qooba.Framework.Bot.Abstractions.Models;
using Qooba.Framework.Bot.Abstractions.Models.Attachments;

namespace Qooba.Framework.Bot
{
    public class ImageReplyBuilder : IReplyBuilder<ImageReplyMessage>
    {
        public async Task<ReplyMessage> ExecuteAsync(IConversationContext context, ImageReplyMessage reply)
        {
            return new ReplyMessage
            {
                Attachment = new ImageAttachment
                {
                    Payload = new MediaAttachmentPayload
                    {
                        Url = reply.Image
                    }
                }
            };
        }
    }

    public class ImageReplyMessage
    {
        public string Image { get; set; }
    }
}
