using Qooba.Framework.Bot.Abstractions.Models;

namespace Qooba.Framework.Bot.Abstractions
{
    public interface IBotConfig
    {
        string MessangerAccessToken { get; }

        string MessangerChallengeVerifyToken { get; }

        string MessangerAppSecret { get; }

        string BotConfigurationPath { get; }
        
        ConnectorType BotConnectorType { get; }

        string BotQueueName { get; }

        string BotQueueConnectionString { get; }

        string BotStateManagerConnectionString { get; }

        string BotConversationContextTableName { get; }

        string BotConversationDataTableName { get; }

        string BotPrivateConversationDataTableName { get; }

        string BotUserDataTableName { get; }
    }
}
