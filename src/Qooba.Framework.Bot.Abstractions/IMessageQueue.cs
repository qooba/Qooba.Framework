using System.Threading.Tasks;

namespace Qooba.Framework.Bot.Abstractions
{
    public interface IMessageQueue
    {
        Task Enqueue(string message);
    }
}
