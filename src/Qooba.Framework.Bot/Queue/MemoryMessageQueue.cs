using Qooba.Framework.Bot.Abstractions;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Qooba.Framework.Bot.Queue
{
    public class MemoryMessageQueue<TMessage> : IMessageQueue<TMessage>
        where TMessage : class
    {
        private static ConcurrentQueue<TMessage> queue = new ConcurrentQueue<TMessage>();

        public async Task<TMessage> Dequeue() => queue.TryDequeue(out TMessage message) ? message : null;

        public async Task Enqueue(TMessage message) => queue.Enqueue(message);

        public async Task<TMessage> Peak() => queue.TryPeek(out TMessage message) ? message : null;
    }
}
