namespace Qooba.Framework.Bot.Connector.Abstractions.Model.Attachments
{
    public abstract class MediaAttachment : Attachment
    {
        public MediaAttachmentPayload Payload { get; set; }
    }
}