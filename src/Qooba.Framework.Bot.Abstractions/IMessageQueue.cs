using System.Threading.Tasks;

namespace Qooba.Framework.Bot.Abstractions
{
    public interface IMessageQueue<TMessage>
        where TMessage : class
    {
        Task Enqueue(TMessage message);

        Task<TMessage> Dequeue();

        Task<TMessage> Peak();
    }
}
