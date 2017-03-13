using System.Threading.Tasks;
using Qooba.Framework.Bot.Abstractions;
using System.Collections.Generic;
using System;
using Qooba.Framework.Logging.Abstractions;
using Qooba.Framework.Bot.Abstractions.Models;

namespace Qooba.Framework.Bot
{
    public class BotImpl : IBot
    {
        private readonly Func<string, IConnector> connectorFactory;

        private readonly ILogger logger;

        public BotImpl(Func<string, IConnector> connectorFactory, ILogger logger)
        {
            this.connectorFactory = connectorFactory;
            this.logger = logger;
        }

        public async Task ProcessAsync(string path, IDictionary<string, string[]> headers, string callback)
        {
            if (Enum.TryParse(path, true, out ConnectorType connectorType))
            {
                var connector = this.connectorFactory(connectorType.ToString());
                //await connector.Process( ReadAsync(headers, callback);
            }
            else
            {
                logger.Error($"Error {nameof(BotImpl)} - connector type {path} not found.");
            }
        }
    }
}
