namespace Qooba.Framework.Bot.Abstractions.Models.Attachments
{
    public class FileAttachment : MediaAttachment
    {
        public override AttachmentType Type => AttachmentType.file;
    }
}