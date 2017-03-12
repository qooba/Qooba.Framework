namespace Qooba.Framework.Bot.Connector.Abstractions.Model.Attachments
{
    public class VideoAttachment : MediaAttachment
    {
        public override AttachmentType Type => AttachmentType.video;
    }
}