using Qooba.Framework.Bot.Abstractions.Models;
using System.Threading.Tasks;

namespace Qooba.Framework.Bot.Abstractions.Form
{
    public interface IFormReplyPropertyConfirmation
    {

    }

    public interface IFormReplyPropertyConfirmation<T> : IFormReplyPropertyConfirmation
        where T : class
    {
        /// <summary>
        /// Check is the response is valid if is invalid return ReplyItem
        /// </summary>
        /// <param name="conversationContext"></param>
        /// <returns></returns>
        Task<ReplyMessage> ConfirmAsync(IConversationContext conversationContext);
    }
}
