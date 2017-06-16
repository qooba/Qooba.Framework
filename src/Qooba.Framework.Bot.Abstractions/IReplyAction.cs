using System.Threading.Tasks;

namespace Qooba.Framework.Bot.Abstractions
{
    public interface IReplyAction<T> : IReplyAction
    {
        Task<T> CreateReplyMessage(IConversationContext conversationContext);
    }

    public interface IReplyAction
    {
    }
}
