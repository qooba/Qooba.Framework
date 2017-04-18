using Qooba.Framework.Bot.Abstractions;
using Qooba.Framework.Bot.Abstractions.Models;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace Qooba.Framework.Bot.Handlers
{
    public class ReplyHandler : BaseHandler, IHandler
    {
        private readonly Func<string, IReplyBuilder> replyBuilders;

        private readonly IReplyConfiguration replyConfiguration;

        private static IDictionary<string, Func<IReplyBuilder, IConversationContext, object, Task<ReplyMessage>>> cachedFunc = new ConcurrentDictionary<string, Func<IReplyBuilder, IConversationContext, object, Task<ReplyMessage>>>();

        public ReplyHandler(IReplyConfiguration replyConfiguration, Func<string, IReplyBuilder> replyBuilders)
        {
            this.replyConfiguration = replyConfiguration;
            this.replyBuilders = replyBuilders;
        }

        public override int Priority => 2;

        public override async Task InvokeAsync(IConversationContext conversationContext)
        {
            var replyItem = await this.replyConfiguration.FetchReplyItem(conversationContext);
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

            conversationContext.Reply = new Reply
            {
                Recipient = new Recipient { Id = conversationContext?.Entry?.Message?.Sender?.Id },
                NotificationType = replyItem.NotificationType,
                SenderAction = replyItem.SenderAction,
                Message = await builderFunc(builder, conversationContext, replyItem.Reply)
            };

            await base.InvokeAsync(conversationContext);
        }
    }
}