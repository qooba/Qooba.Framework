using Qooba.Framework.Bot.Abstractions.Models.Templates;

namespace Qooba.Framework.Bot.Abstractions.Models.Attachments
{
    public class TemplateAttachment : Attachment
    {
        public AttachmentType Type { get; set; } = AttachmentType.template;

        public TemplateAttachmentPayload Payload { get; set; }
    }
}