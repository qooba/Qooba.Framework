namespace Qooba.Framework.Bot.Abstractions.Models.Attachments
{
    public abstract class MediaAttachment : Attachment
    {
        public MediaAttachmentPayload Payload { get; set; }
    }
}