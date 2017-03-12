namespace Qooba.Framework.Bot.Connector.Abstractions.Model.Attachments
{
    public class FileAttachment : MediaAttachment
    {
        public override AttachmentType Type => AttachmentType.file;
    }
}