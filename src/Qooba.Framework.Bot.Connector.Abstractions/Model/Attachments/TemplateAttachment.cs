using Qooba.Framework.Bot.Connector.Abstractions.Model.Templates;

namespace Qooba.Framework.Bot.Connector.Abstractions.Model.Attachments
{
    public class TemplateAttachment : Attachment
    {
        public override AttachmentType Type => AttachmentType.template;

        public TemplateAttachmentPayload Payload { get; set; }
    }
}