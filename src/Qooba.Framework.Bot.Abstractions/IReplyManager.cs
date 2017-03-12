using Qooba.Framework.Bot.Abstractions.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Qooba.Framework.Bot.Abstractions
{
    public interface IReplyManager
    {
        Task<Reply> CreateAsync(IConversationContext context);

        Task<IList<Route>> FetchRoutingTableAsync();
    }
}

