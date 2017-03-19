using Qooba.Framework.Bot.Abstractions;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Qooba.Framework.Bot.Queue
{
    public class MemoryMessageQueue : IMessageQueue
    {
        private static ConcurrentQueue<string> queue = new ConcurrentQueue<string>();
        
        public async Task Enqueue(string message) => queue.Enqueue(message);
    }
}
