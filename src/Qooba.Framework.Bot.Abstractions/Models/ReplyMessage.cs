using Qooba.Framework.Bot.Abstractions.Models.Attachments;

namespace Qooba.Framework.Bot.Abstractions.Models
{
    public class ReplyMessage : BaseMessage
    {
        public Attachment Attachment { get; set; }
    }
}
