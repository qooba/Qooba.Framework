namespace Qooba.Framework.Bot.Abstractions.Models.Attachments
{
    public class ImageAttachment : MediaAttachment
    {
        public AttachmentType Type { get; set; } = AttachmentType.image;
    }
}