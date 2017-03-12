using System.Threading.Tasks;

namespace Qooba.Framework.Bot.Abstractions
{
    public interface IHandlerManager
    {
        Task<IHandler> CreateAsync(IConversationContext conversationContext);
    }
}