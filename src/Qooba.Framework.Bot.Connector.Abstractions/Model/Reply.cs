namespace Qooba.Framework.Bot.Connector.Abstractions.Model
{
    public class Reply
    {
        public Recipient Recipient { get; set; }

        public ReplyMessage Message { get; set; }

        public SenderAction? SenderAction { get; set; }

        public NotificationType NotificationType { get; set; }
    }
}
