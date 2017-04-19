using System.Threading.Tasks;

namespace Qooba.Framework.Bot.Abstractions
{
    public interface IStateManager
    {
        Task<IConversationContext> FetchContextAsync(IConversationContext context);

        Task SaveContextAsync(IConversationContext context);
    }
}