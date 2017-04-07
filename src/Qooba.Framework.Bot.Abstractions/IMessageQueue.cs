using System.Threading.Tasks;

namespace Qooba.Framework.Bot.Abstractions
{
    public interface IMessageQueue
    {
        Task EnqueueAsync(string message);
    }
}
