using Qooba.Framework.Bot.Abstractions.Models;
using System.Threading.Tasks;

namespace Qooba.Framework.Bot.Abstractions.Form
{
    public interface IFormReplyPropertyValidator
    {

    }

    public interface IFormReplyPropertyValidator<T>
        where T : class
    {
        /// <summary>
        /// Check is the response is valid if is invalid return ReplyItem
        /// </summary>
        /// <param name="conversationContext"></param>
        /// <returns></returns>
        Task<ReplyMessage> CheckValidAsync(IConversationContext conversationContext);
    }
}
