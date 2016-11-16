using System.Collections.Generic;
using System.Threading.Tasks;

namespace Qooba.Framework.Azure.Storage.Abstractions
{
    public interface IAzureBlobQueue
    {
        Task CreateQueue(string queueName);

        Task DeleteQueue(string queueName);

        Task ClearQueue(string queueName);

        Task AddMessage(string queueName, string content);

        Task AddMessage(string queueName, byte[] content);

        Task<string> GetMessageString(string queueName);

        Task<byte[]> GetMessageBytes(string queueName);

        Task<IList<string>> GetMessagesString(string queueName, int messagesCount);

        Task<IList<byte[]>> GetMessagesBytes(string queueName, int messagesCount);

        Task<string> PeekMessageString(string queueName);

        Task<byte[]> PeekMessageBytes(string queueName);

        Task<IList<string>> PeekMessagesString(string queueName, int messagesCount);

        Task<IList<byte[]>> PeekMessagesBytes(string queueName, int messagesCount);

        Task DeleteMessage(string queueName, string messageId, string popReceipt);
    }
}
