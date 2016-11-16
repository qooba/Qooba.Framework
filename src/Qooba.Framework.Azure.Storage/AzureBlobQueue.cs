using System.Threading.Tasks;
using Qooba.Framework.Configuration.Abstractions;
using System;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Qooba.Framework.Azure.Storage.Abstractions;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Microsoft.WindowsAzure.Storage.Queue;

namespace Qooba.Framework.Azure.Storage
{
    public class AzureBlobQueue : IAzureBlobQueue
    {
        private readonly IConfig config;

        public AzureBlobQueue(IConfig config)
        {
            this.config = config;
        }

        public async Task CreateQueue(string queueName)
        {
            await PrepareQueue(queueName).CreateIfNotExistsAsync();
        }

        public async Task DeleteQueue(string queueName)
        {
            await PrepareQueue(queueName).DeleteIfExistsAsync();
        }

        public async Task ClearQueue(string queueName)
        {
            await PrepareQueue(queueName).ClearAsync();
        }

        public async Task AddMessage(string queueName, string content)
        {
            await PrepareQueue(queueName).AddMessageAsync(new CloudQueueMessage(content));
        }

        public async Task AddMessage(string queueName, byte[] content)
        {
            await PrepareQueue(queueName).AddMessageAsync(new CloudQueueMessage(content));
        }

        public async Task<string> GetMessageString(string queueName)
        {
            var result = await PrepareQueue(queueName).GetMessageAsync();
            return result.AsString;
        }

        public async Task<byte[]> GetMessageBytes(string queueName)
        {
            var result = await PrepareQueue(queueName).GetMessageAsync();
            return result.AsBytes;
        }

        public async Task<IList<string>> GetMessagesString(string queueName, int messagesCount)
        {
            var result = await PrepareQueue(queueName).GetMessagesAsync(messagesCount);
            return result.Select(x => x.AsString).ToList();
        }

        public async Task<IList<byte[]>> GetMessagesBytes(string queueName, int messagesCount)
        {
            var result = await PrepareQueue(queueName).GetMessagesAsync(messagesCount);
            return result.Select(x => x.AsBytes).ToList();
        }

        public async Task<string> PeekMessageString(string queueName)
        {
            var result = await PrepareQueue(queueName).PeekMessageAsync();
            return result.AsString;
        }

        public async Task<byte[]> PeekMessageBytes(string queueName)
        {
            var result = await PrepareQueue(queueName).PeekMessageAsync();
            return result.AsBytes;
        }

        public async Task<IList<string>> PeekMessagesString(string queueName, int messagesCount)
        {
            var result = await PrepareQueue(queueName).PeekMessagesAsync(messagesCount);
            return result.Select(x => x.AsString).ToList();
        }

        public async Task<IList<byte[]>> PeekMessagesBytes(string queueName, int messagesCount)
        {
            var result = await PrepareQueue(queueName).PeekMessagesAsync(messagesCount);
            return result.Select(x => x.AsBytes).ToList();
        }

        public async Task DeleteMessage(string queueName, string messageId, string popReceipt)
        {
            await PrepareQueue(queueName).DeleteMessageAsync(new CloudQueueMessage(messageId, popReceipt));
        }

        private CloudQueue PrepareQueue(string queueName)
        {
            var storageAccount = CloudStorageAccount.Parse(this.config.StorageConnectionString);
            var queueClient = storageAccount.CreateCloudQueueClient();
            return queueClient.GetQueueReference(queueName);
        }

    }
}
