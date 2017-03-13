using Qooba.Framework.Bot.Abstractions;
using System.Threading.Tasks;
namespace Qooba.Framework.Bot.Handlers
{
    public abstract class BaseHandler : IHandler
    {
        public abstract int Priority { get; }

        public IHandler NextHandler { get; set; }

        public virtual async Task InvokeAsync(IConversationContext conversationContext)
        {
            if (this.NextHandler != null)
            {
                await this.NextHandler.InvokeAsync(conversationContext);
            }
        }
    }
}