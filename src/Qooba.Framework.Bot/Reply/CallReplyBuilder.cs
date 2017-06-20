using Qooba.Framework.Bot.Abstractions;
using System.Threading.Tasks;
using Qooba.Framework.Bot.Abstractions.Models;
using Qooba.Framework.Bot.Abstractions.Models.Attachments;
using Qooba.Framework.Bot.Abstractions.Models.Templates;
using Qooba.Framework.Bot.Abstractions.Models.Buttons;

namespace Qooba.Framework.Bot
{
    public class CallReplyBuilder : IReplyBuilder<CallReplyMessage>
    {
        public async Task<ReplyMessage> ExecuteAsync(IConversationContext context, CallReplyMessage reply)
        {
            return new ReplyMessage
            {
                Attachment = new TemplateAttachment
                {
                    Payload = new ButtonTemplateAttachmentPayload
                    {
                        Text = reply.Text,
                        Buttons = new[] { new CallButton { Payload = reply.PhoneNumber, Title = reply.Title } }
                    }
                }
            };
        }
    }

    public class CallReplyMessage
    {
        public string Text { get; set; }

        public string Title { get; set; }

        public string PhoneNumber { get; set; }
    }
}
