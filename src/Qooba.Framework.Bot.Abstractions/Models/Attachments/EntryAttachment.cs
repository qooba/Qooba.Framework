namespace Qooba.Framework.Bot.Abstractions.Models.Attachments
{
    public class EntryAttachment
    {
        public AttachmentType Type { get; set; }

        public MediaAttachmentPayload Payload { get; set; }

        public string AttachmentData { get; set; }
    }
}