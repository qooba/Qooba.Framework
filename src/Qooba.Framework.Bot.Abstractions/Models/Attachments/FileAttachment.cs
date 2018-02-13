namespace Qooba.Framework.Bot.Abstractions.Models.Attachments
{
    public class FileAttachment : MediaAttachment
    {
        public AttachmentType Type { get; set; } = AttachmentType.file;
    }
}