using Qooba.Framework.Bot.Abstractions.Models;
using System.Threading.Tasks;

namespace Qooba.Framework.Bot.Abstractions
{
    public interface IBot
    {
        Task Run(string queueItem);

        Task Run(Entry entry);

        Task Redirect(string routeText, IConversationContext conversationContext);
    }
}
