namespace Qooba.Framework.Bot.Abstractions.Models.Attachments
{
    public class VideoAttachment : MediaAttachment
    {
        public override AttachmentType Type => AttachmentType.video;
    }
}