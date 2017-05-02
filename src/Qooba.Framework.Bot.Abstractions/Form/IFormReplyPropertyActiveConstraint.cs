using System.Threading.Tasks;

namespace Qooba.Framework.Bot.Abstractions.Form
{
    public interface IFormReplyPropertyActiveConstraint
    {

    }

    public interface IFormReplyPropertyActiveConstraint<T> : IFormReplyPropertyActiveConstraint
        where T : class
    {
        /// <summary>
        /// Check if the property will be visible
        /// </summary>
        /// <param name="conversationContext"></param>
        /// <returns></returns>
        Task<bool> CheckActiveAsync(IConversationContext conversationContext);
    }
}
