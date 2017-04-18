using System.Threading.Tasks;

namespace Qooba.Framework.Bot.Abstractions
{
    public interface IStateManager
    {
        Task<IConversationContext> FetchContextAsync(IConversationContext context);

        Task SaveContextAsync(IConversationContext context);

        Task<T> FetchUserDataAsync<T>(string key) where T : class;

        Task<T> FetchConversationDataAsync<T>(string key) where T : class;

        Task<T> FetchPrivateConversationDataAsync<T>(string key) where T : class;

        Task SaveUserDataAsync<T>(string key, T data) where T : class;

        Task SaveConversationDataAsync<T>(string key, T data) where T : class;

        Task SavePrivateConversationDataAsync<T>(string key, T data) where T : class;
    }
}