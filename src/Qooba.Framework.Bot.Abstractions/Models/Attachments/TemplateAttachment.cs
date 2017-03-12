using Qooba.Framework.Bot.Abstractions.Models.Templates;

namespace Qooba.Framework.Bot.Abstractions.Models.Attachments
{
    public class TemplateAttachment : Attachment
    {
        public override AttachmentType Type => AttachmentType.template;

        public TemplateAttachmentPayload Payload { get; set; }
    }
}