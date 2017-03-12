using Qooba.Framework.Bot.Abstractions.Model;
using Qooba.Framework.Bot.Abstractions.Models;

namespace Qooba.Framework.Bot.Abstractions
{
    public interface IConversationContext
    {
        Route Route { get; set; }

        User User { get; set; }

        ConnectorType ConnectorType { get; set; }

        Entry Entry { get; set; }

        Reply Reply { get; set; }

        bool KeepState { get; set; }
    }
}