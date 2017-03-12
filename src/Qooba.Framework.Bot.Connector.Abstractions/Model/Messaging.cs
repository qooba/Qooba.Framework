namespace Qooba.Framework.Bot.Connector.Abstractions.Model
{
    public class Messaging
    {
        public Sender Sender { get; set; }

        public Recipient Recipient { get; set; }

        public int Timestamp { get; set; }

        public EntryMessage Message { get; set; }
    }
}
