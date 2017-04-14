using Qooba.Framework.Bot.Abstractions.Models;
using System.Threading.Tasks;

namespace Qooba.Framework.Bot.Abstractions
{
    public interface IReplyConfiguration
    {
        Task<ReplyItem> FetchReplyItem(IConversationContext context);
    }
}

