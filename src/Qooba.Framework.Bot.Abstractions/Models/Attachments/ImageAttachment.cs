namespace Qooba.Framework.Bot.Abstractions.Models.Attachments
{
    public class ImageAttachment : MediaAttachment
    {
        public override AttachmentType Type => AttachmentType.image;
    }
}