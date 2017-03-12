namespace Qooba.Framework.Bot.Abstractions.Models
{
    public class Reply
    {
        public Recipient Recipient { get; set; }

        public ReplyMessage Message { get; set; }

        public SenderAction? SenderAction { get; set; }

        public NotificationType NotificationType { get; set; }
    }
}
