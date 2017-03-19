using System.Threading.Tasks;
using Qooba.Framework.Bot.Abstractions;
using Qooba.Framework.Azure.Storage.Abstractions;
using Qooba.Framework.Configuration.Abstractions;

namespace Qooba.Framework.Bot.Azure
{
    public class AzureMessageQueue : IMessageQueue
    {
        private readonly IAzureBlobQueue azureBlobQueue;

        private readonly IConfig config;

        public AzureMessageQueue(IAzureBlobQueue azureBlobQueue, IConfig config)
        {
            this.azureBlobQueue = azureBlobQueue;
            this.config = config;
        }

        public Task Enqueue(string message) => this.azureBlobQueue.AddMessage(this.config["QueueName"], message);
    }
}