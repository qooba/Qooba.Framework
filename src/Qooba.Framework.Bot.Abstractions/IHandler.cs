using System.Threading.Tasks;

namespace Qooba.Framework.Bot.Abstractions
{
    public interface IHandler
    {
        int Priority { get; }

        IHandler NextHandler { get; set; }

        Task InvokeAsync(IConversationContext conversationContext);
    }
}