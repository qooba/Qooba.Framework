using Qooba.Framework.Bot.Abstractions;
using System.Threading.Tasks;
using Qooba.Framework.Bot.Abstractions.Models;
using Qooba.Framework.Bot.Abstractions.Models.Attachments;

namespace Qooba.Framework.Bot
{
    public class VideoReplyBuilder : IReplyBuilder<VideoReplyMessage>
    {
        public async Task<ReplyMessage> BuildAsync(IConversationContext context, VideoReplyMessage reply)
        {
            return new ReplyMessage
            {
                Attachment = new VideoAttachment
                {
                    Payload = new MediaAttachmentPayload
                    {
                        Url = reply.Video
                    }
                }
            };
        }
    }

    public class VideoReplyMessage
    {
        public string Video { get; set; }
    }
}
