namespace Qooba.Framework.Bot.Connector.Abstractions.Model.Attachments
{
    public class AudioAttachment : MediaAttachment
    {
        public override AttachmentType Type => AttachmentType.audio;
    }
}