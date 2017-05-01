using Qooba.Framework.Bot.Abstractions.Models;
using System.Threading.Tasks;

namespace Qooba.Framework.Bot.Abstractions.Form
{
    public interface IFormReplyCompletionAction
    {

    }

    public interface IFormReplyCompletionAction<T>
        where T : class
    {
        /// <summary>
        /// Action which will be executed when the form is complete
        /// </summary>
        /// <param name="conversationContext"></param>
        /// <param name="completionActionData"></param>
        /// <returns></returns>
        Task<ReplyMessage> ExecuteAsync(IConversationContext conversationContext, T completionActionData);
    }
}
