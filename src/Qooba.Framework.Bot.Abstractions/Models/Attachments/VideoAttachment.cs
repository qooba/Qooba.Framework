namespace Qooba.Framework.Bot.Abstractions.Models.Attachments
{
    public class VideoAttachment : MediaAttachment
    {
        public AttachmentType Type { get; set; } = AttachmentType.video;
    }
}