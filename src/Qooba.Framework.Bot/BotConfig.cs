using System;
using Qooba.Framework.Bot.Abstractions;
using Qooba.Framework.Bot.Abstractions.Models;
using Qooba.Framework.Configuration.Abstractions;

namespace Qooba.Framework.Bot
{
    public class BotConfig : IBotConfig
    {
        private readonly IConfig config;

        public BotConfig(IConfig config)
        {
            this.config = config;
        }

        public string MessangerAccessToken => this.config[Constants.MessangerAccessToken];

        public string BotConfigurationPath => this.config[Constants.BotConfigurationPath];

        public string MessangerChallengeVerifyToken => this.config[Constants.MessangerChallengeVerifyToken];

        public string MessangerAppSecret => this.config[Constants.MessangerAppSecret];

        public ConnectorType BotConnectorType => Enum.TryParse(this.config[Constants.BotConnectorType], out ConnectorType connectorType) ? connectorType : ConnectorType.Messanger;

        public string BotQueueName => this.config[Constants.BotQueueName];

        public string BotQueueConnectionString => this.config[Constants.BotQueueConnectionString];

        public string BotStateManagerConnectionString => this.config[Constants.BotStateManagerConnectionString];

        public string BotConversationContextTableName => this.config[Constants.BotConversationContextTableName];

        public string BotUserProfileConnectionString => this.config[Constants.BotUserProfileConnectionString];

        public string BotUserProfileTableName => this.config[Constants.BotUserProfileTableName];
    }
}
