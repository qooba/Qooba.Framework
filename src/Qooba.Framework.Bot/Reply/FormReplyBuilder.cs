using Qooba.Framework.Bot.Abstractions;
using System.Threading.Tasks;
using Qooba.Framework.Bot.Abstractions.Models;
using Qooba.Framework.Bot.Abstractions.Models.Attachments;
using System.Collections;
using System.Collections.Generic;

namespace Qooba.Framework.Bot
{
    public class FormReplyBuilder : IReplyBuilder<ImageReplyMessage>
    {
        public async Task<ReplyMessage> BuildAsync(IConversationContext context, ImageReplyMessage reply)
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

    public class FormReplyMessage
    {
        public IEnumerable<FormReplyMessageProperty> Properties { get; set; }
    }

    public class FormReplyMessageProperty
    {
        public string PropertyName { get; set; }

        public string PropertyType { get; set; }

        public ReplyItem ReplyItem { get; set; }
    }
}
