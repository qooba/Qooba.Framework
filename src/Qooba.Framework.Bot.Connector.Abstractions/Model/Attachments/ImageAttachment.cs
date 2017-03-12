namespace Qooba.Framework.Bot.Connector.Abstractions.Model.Attachments
{
    public class ImageAttachment : MediaAttachment
    {
        public override AttachmentType Type => AttachmentType.image;
    }
}