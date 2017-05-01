using Qooba.Framework.Bot.Abstractions;
using System;
using Qooba.Framework.Bot.Abstractions.Models;
using System.Threading.Tasks;
using System.Linq;

namespace Qooba.Framework.Bot
{
    public class ReplyFactory : IReplyFactory
    {
        private readonly Func<object, IReplyBuilder> replyBuilders;

        private readonly IGenericExpressionFactory genericExpressionFactory;

        public ReplyFactory(Func<object, IReplyBuilder> replyBuilders, IGenericExpressionFactory genericExpressionFactory)
        {
            this.replyBuilders = replyBuilders;
            this.genericExpressionFactory = genericExpressionFactory;
        }

        public async Task<Reply> CreateReplyAsync(IConversationContext conversationContext, ReplyItem replyItem)
        {
            var replyItemText = replyItem.Reply.ToString();

            conversationContext.Route.RouteData?.ToList().ForEach(d =>
            {
                replyItemText = replyItemText.Replace(string.Concat("{{", d.Key, "}}"), d.Value.ToString());
            });

            var message = await (Task<ReplyMessage>)this.genericExpressionFactory.Create(replyItem.ReplyType, replyBuilders, conversationContext, replyItemText);

            return new Reply
            {
                Recipient = new Recipient { Id = conversationContext?.Entry?.Message?.Sender?.Id },
                NotificationType = replyItem.NotificationType,
                SenderAction = replyItem.SenderAction,
                Message = message
            };
        }
    }
}
