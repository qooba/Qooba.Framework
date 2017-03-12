using System.Threading.Tasks;

namespace Qooba.Framework.Bot.Abstractions
{
    public interface IStateManager
    {
        Task<IConversationContext> FetchContext(IConversationContext context);

        Task SaveContext(IConversationContext context);
    }
}