using System.Collections.Generic;
using System.Threading.Tasks;

namespace Qooba.Framework.Bot.Abstractions
{
    public interface IReplyAction<T> : IReplyAction
    {
        Task<T> CreateReplyMessage(IConversationContext conversationContext, IDictionary<string, string> parameters);
    }

    public interface IReplyAction
    {
    }
}
