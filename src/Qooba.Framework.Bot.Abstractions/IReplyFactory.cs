using Qooba.Framework.Bot.Abstractions.Models;
using System.Threading.Tasks;

namespace Qooba.Framework.Bot.Abstractions
{
    public interface IReplyFactory
    {
        Task<Reply> CreateReplyAsync(IConversationContext conversationContext, ReplyItem replyItem);
    }
}
