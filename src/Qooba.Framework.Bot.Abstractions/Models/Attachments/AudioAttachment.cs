namespace Qooba.Framework.Bot.Abstractions.Models.Attachments
{
    public class AudioAttachment : MediaAttachment
    {
        public override AttachmentType Type => AttachmentType.audio;
    }
}