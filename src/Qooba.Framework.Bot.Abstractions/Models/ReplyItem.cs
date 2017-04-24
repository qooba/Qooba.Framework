using System.Collections.Generic;

namespace Qooba.Framework.Bot.Abstractions.Models
{
    public class ReplyItem
    {
        public string ReplyId { get; set; }

        public IList<string> Routes { get; set; }

        public string ReplyType { get; set; }

        public SenderAction? SenderAction { get; set; }

        public NotificationType NotificationType { get; set; }

        public object Reply { get; set; }

        public bool IsDefault { get; set; }
    }
}