using Qooba.Framework.Bot.Abstractions;
using System;
using Qooba.Framework.Bot.Abstractions.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Reflection;
using System.Linq.Expressions;
using System.Linq;
using Qooba.Framework.Serialization.Abstractions;

namespace Qooba.Framework.Bot
{
    public class ReplyFactory : IReplyFactory
    {
        private static IDictionary<string, Func<IReplyBuilder, IConversationContext, object, Task<ReplyMessage>>> cachedFunc = new ConcurrentDictionary<string, Func<IReplyBuilder, IConversationContext, object, Task<ReplyMessage>>>();

        private static IDictionary<string, Type> cachedReplyType = new ConcurrentDictionary<string, Type>();

        private readonly Func<object, IReplyBuilder> replyBuilders;

        private readonly ISerializer serializer;

        public ReplyFactory(Func<object, IReplyBuilder> replyBuilders, ISerializer serializer)
        {
            this.replyBuilders = replyBuilders;
            this.serializer = serializer;
        }

        public async Task<Reply> CreateReplyAsync(IConversationContext conversationContext, ReplyItem replyItem)
        {
            var builder = replyBuilders(replyItem.ReplyType);
            Func<IReplyBuilder, IConversationContext, object, Task<ReplyMessage>> builderFunc = null;

            if (!cachedFunc.TryGetValue(replyItem.ReplyType, out builderFunc))
            {
                foreach (var i in builder.GetType().GetTypeInfo().GetInterfaces())
                {
                    if (i.GetTypeInfo().IsGenericType && i.GetGenericTypeDefinition() == typeof(IReplyBuilder<>))
                    {
                        var type = i.GetGenericArguments().FirstOrDefault();
                        var method = i.GetMethods().FirstOrDefault(x => x.Name == "BuildAsync");
                        cachedReplyType[replyItem.ReplyType] = type;
                        var build = Expression.Parameter(typeof(IReplyBuilder), "builder");
                        var context = Expression.Parameter(typeof(IConversationContext), "conversationContext");
                        var reply = Expression.Parameter(typeof(object), "reply");

                        var builderFuncExpression = Expression.Lambda<Func<IReplyBuilder, IConversationContext, object, Task<ReplyMessage>>>(Expression.Call(Expression.Convert(build, i), method, context, Expression.Convert(reply, type)), build, context, reply);
                        builderFunc = builderFuncExpression.Compile();
                        cachedFunc[replyItem.ReplyType] = builderFunc;
                        break;
                    }
                }
            }

            var replyItemText = replyItem.Reply.ToString();

            conversationContext.Route.RouteData?.ToList().ForEach(d =>
            {
                replyItemText = replyItemText.Replace(string.Concat("{{", d.Key, "}}"), d.Value.ToString());
            });

            var replyObject = this.serializer.Deserialize(replyItemText, cachedReplyType[replyItem.ReplyType]);

            return new Reply
            {
                Recipient = new Recipient { Id = conversationContext?.Entry?.Message?.Sender?.Id },
                NotificationType = replyItem.NotificationType,
                SenderAction = replyItem.SenderAction,
                Message = await builderFunc(builder, conversationContext, replyObject)
            };
        }
    }
}
