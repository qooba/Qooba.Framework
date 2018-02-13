namespace Qooba.Framework.Bot.Abstractions.Models.Attachments
{
    public class AudioAttachment : MediaAttachment
    {
        public AttachmentType Type { get; set; } = AttachmentType.audio;
    }
}