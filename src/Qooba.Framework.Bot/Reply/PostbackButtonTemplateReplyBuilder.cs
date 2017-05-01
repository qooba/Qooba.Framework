using Qooba.Framework.Bot.Abstractions;
using System.Threading.Tasks;
using Qooba.Framework.Bot.Abstractions.Models;
using Qooba.Framework.Bot.Abstractions.Models.Attachments;
using Qooba.Framework.Bot.Abstractions.Models.Templates;
using Qooba.Framework.Bot.Abstractions.Models.Buttons;
using System.Collections.Generic;
using System.Linq;

namespace Qooba.Framework.Bot
{
    public class PostbackButtonTemplateReplyBuilder : IReplyBuilder<PostbackButtonTemplateReplyMessage>
    {
        public async Task<ReplyMessage> ExecuteAsync(IConversationContext context, PostbackButtonTemplateReplyMessage reply)
        {
            return new ReplyMessage
            {
                Attachment = new TemplateAttachment
                {
                    Payload = new ButtonTemplateAttachmentPayload
                    {
                        Text = reply.Text,
                        Buttons = reply.Buttons.Cast<Button>().ToList()
                    }
                }
            };
        }
    }

    public class PostbackButtonTemplateReplyMessage
    {
        public string Text { get; set; }

        public IList<PostbackButton> Buttons { get; set; }
    }
}
