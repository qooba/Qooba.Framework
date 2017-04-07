using System.Threading.Tasks;
using Qooba.Framework.Bot.Abstractions;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage;

namespace Qooba.Framework.Bot.Azure
{
    public class AzureMessageQueue : IMessageQueue
    {
        private readonly IBotConfig config;

        public AzureMessageQueue(IBotConfig config)
        {
            this.config = config;
        }

        public Task EnqueueAsync(string message) => this.PrepareQueue().AddMessageAsync(new CloudQueueMessage(message));

        private CloudQueue PrepareQueue()
        {
            var storageAccount = CloudStorageAccount.Parse(this.config.BotQueueConnectionString);
            var queueClient = storageAccount.CreateCloudQueueClient();
            return queueClient.GetQueueReference(this.config.BotQueueName);
        }
    }
}