using System.Threading.Tasks;
using Qooba.Framework.Bot.Abstractions;
using Amazon.SimpleNotificationService;

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

        public async Task EnqueueAsync(string message) => await this.client.PublishAsync(this.config.BotQueueName, message);
    }
}