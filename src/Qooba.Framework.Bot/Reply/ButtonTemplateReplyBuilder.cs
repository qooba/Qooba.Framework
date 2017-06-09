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
    public class ButtonTemplateReplyBuilder : IReplyBuilder<ButtonTemplateReplyMessage>
    {
        public async Task<ReplyMessage> ExecuteAsync(IConversationContext context, ButtonTemplateReplyMessage reply)
        {
            return new ReplyMessage
            {
                Attachment = new TemplateAttachment
                {
                    Payload = new ButtonTemplateAttachmentPayload
                    {
                        Text = reply.Text,
                        Buttons = reply.Buttons.Select(b => b.ToButton()).ToList()
                    }
                }
            };
        }
    }

    public class ButtonTemplateReplyMessage
    {
        public string Text { get; set; }

        public IList<FullButton> Buttons { get; set; }
    }
}
