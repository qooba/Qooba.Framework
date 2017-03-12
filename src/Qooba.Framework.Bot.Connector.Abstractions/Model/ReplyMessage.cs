using Qooba.Framework.Bot.Connector.Abstractions.Model.Attachments;

namespace Qooba.Framework.Bot.Connector.Abstractions.Model
{
    public class ReplyMessage : BaseMessage
    {
        public Attachment Attachment { get; set; }
    }
}
