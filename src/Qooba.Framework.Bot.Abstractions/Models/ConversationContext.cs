namespace Qooba.Framework.Bot.Abstractions.Models
{
    public class ConversationContext : IConversationContext
    {
        public Route Route { get; set; }

        public User User { get; set; }

        public ConnectorType ConnectorType { get; set; }

        public Entry Entry { get; set; }

        public Reply Reply { get; set; }

        public bool KeepState { get; set; }
    }
}