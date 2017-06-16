using System.Threading.Tasks;
using Qooba.Framework.Bot.Abstractions;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;

namespace Qooba.Framework.Bot.Aws
{
    public class AwsMessageQueue : IMessageQueue
    {
        private readonly IBotConfig config;

        private readonly IAmazonSimpleNotificationService client;

        public AwsMessageQueue(IBotConfig config)
        {
            this.config = config;
            this.client = new AmazonSimpleNotificationServiceClient();
        }

        public Task EnqueueAsync(string message) => this.PrepareQueue().AddMessageAsync(new CloudQueueMessage(message));

        private CloudQueue PrepareQueue()
        {
            
            client.PublishAsync
            //var storageAccount = CloudStorageAccount.Parse(this.config.BotQueueConnectionString);
            //var queueClient = storageAccount.CreateCloudQueueClient();
            //return queueClient.GetQueueReference(this.config.BotQueueName);
        }
    }
}