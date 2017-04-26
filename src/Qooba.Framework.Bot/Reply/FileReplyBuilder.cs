using Qooba.Framework.Bot.Abstractions;
using System.Threading.Tasks;
using Qooba.Framework.Bot.Abstractions.Models;
using Qooba.Framework.Bot.Abstractions.Models.Attachments;

namespace Qooba.Framework.Bot
{
    public class FileReplyBuilder : IReplyBuilder<FileReplyMessage>
    {
        public async Task<ReplyMessage> BuildAsync(IConversationContext context, FileReplyMessage reply)
        {
            return new ReplyMessage
            {
                Attachment = new FileAttachment
                {
                    Payload = new MediaAttachmentPayload
                    {
                        Url = reply.File
                    }
                }
            };
        }
    }

    public class FileReplyMessage
    {
        public string File { get; set; }
    }
}
