using Qooba.Framework.Bot.Abstractions;
using Qooba.Framework.Bot.Abstractions.Models;
using Qooba.Framework.Serialization.Abstractions;
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
        private readonly Func<object, IReplyBuilder> replyBuilders;

        private readonly IReplyConfiguration replyConfiguration;

        private readonly ISerializer serializer;

        private static IDictionary<string, Func<IReplyBuilder, IConversationContext, object, Task<ReplyMessage>>> cachedFunc = new ConcurrentDictionary<string, Func<IReplyBuilder, IConversationContext, object, Task<ReplyMessage>>>();

        private static IDictionary<string, Type> cachedReplyType = new ConcurrentDictionary<string, Type>();

        public ReplyHandler(IReplyConfiguration replyConfiguration, Func<object, IReplyBuilder> replyBuilders, ISerializer serializer)
        {
            this.replyConfiguration = replyConfiguration;
            this.replyBuilders = replyBuilders;
            this.serializer = serializer;
        }

        public override int Priority => 3;

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

            conversationContext.Reply = new Reply
            {
                Recipient = new Recipient { Id = conversationContext?.Entry?.Message?.Sender?.Id },
                NotificationType = replyItem.NotificationType,
                SenderAction = replyItem.SenderAction,
                Message = await builderFunc(builder, conversationContext, replyObject)
            };

            await base.InvokeAsync(conversationContext);
        }
    }
}