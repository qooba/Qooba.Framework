using Qooba.Framework.Bot.Abstractions;
using Qooba.Framework.Bot.Abstractions.Form;
using Qooba.Framework.Bot.Abstractions.Models;
using System.Threading.Tasks;

namespace Qooba.Framework.Bot.Form
{
    public class TextFormReplyCompletionAction : IFormReplyCompletionAction<TextFormReplyCompletionActionData>
    {
        public async virtual Task<ReplyMessage> ExecuteAsync(IConversationContext conversationContext, TextFormReplyCompletionActionData completionActionData)
        {
            return new ReplyMessage
            {
                Text = completionActionData.Text
            };
        }
    }

    public class TextFormReplyCompletionActionData
    {
        public string Text { get; set; }
    }
}
