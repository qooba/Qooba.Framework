using Qooba.Framework.Bot.Abstractions;
using Qooba.Framework.Serialization.Abstractions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Qooba.Framework.Bot
{
    public class GenericExpressionFactory : IGenericExpressionFactory
    {
        private const string MethodName = "ExecuteAsync";

        private static IDictionary<Type, ConcurrentDictionary<string, object>> cachedFunc = new ConcurrentDictionary<Type, ConcurrentDictionary<string, object>>();

        private static IDictionary<Type, ConcurrentDictionary<string, Type>> cachedReplyType = new ConcurrentDictionary<Type, ConcurrentDictionary<string, Type>>();

        private readonly ISerializer serializer;

        public GenericExpressionFactory(ISerializer serializer)
        {
            this.serializer = serializer;
        }

        public object Create<TInterface>(string itemType, Func<string, TInterface> itemFactory, IConversationContext conversationContext, string itemDataText)
        {
            conversationContext?.Route?.RouteData?.ToList().ForEach(d =>
            {
                itemDataText = itemDataText.Replace(string.Concat("{{", d.Key, "}}"), d.Value.ToString());
            });

            var item = itemFactory(itemType);
            Func<TInterface, IConversationContext, object, object> itemFunc = null;
            var typeInterface = typeof(TInterface);

            if (!cachedFunc.TryGetValue(typeInterface, out ConcurrentDictionary<string, object> cachedSubFunc))
            {
                cachedSubFunc = cachedFunc[typeInterface] = new ConcurrentDictionary<string, object>();
            }

            if (!cachedReplyType.TryGetValue(typeInterface, out ConcurrentDictionary<string, Type> cachedSubReplyType))
            {
                cachedSubReplyType = cachedReplyType[typeInterface] = new ConcurrentDictionary<string, Type>();
            }

            if (!cachedSubFunc.TryGetValue(itemType, out object itemFuncObj))
            {
                foreach (var i in item.GetType().GetTypeInfo().GetInterfaces())
                {
                    if (i.GetTypeInfo().IsGenericType && i.GetGenericTypeDefinition().Name.Contains(typeInterface.Name))
                    {
                        var type = i.GetGenericArguments().FirstOrDefault();
                        var method = i.GetMethods().FirstOrDefault(x => x.Name == MethodName);
                        cachedSubReplyType[itemType] = type;
                        var build = Expression.Parameter(typeInterface, "item");
                        var context = Expression.Parameter(typeof(IConversationContext), "conversationContext");
                        var reply = Expression.Parameter(typeof(object), "reply");

                        var builderFuncExpression = Expression.Lambda<Func<TInterface, IConversationContext, object, object>>(Expression.Convert(Expression.Call(Expression.Convert(build, i), method, context, Expression.Convert(reply, type)), typeof(object)), build, context, reply);
                        itemFunc = builderFuncExpression.Compile();
                        cachedSubFunc[itemType] = itemFunc;
                        break;
                    }
                }
            }
            else
            {
                itemFunc = (Func<TInterface, IConversationContext, object, object>)itemFuncObj;
            }

            var itemDataObj = this.serializer.Deserialize(itemDataText, cachedSubReplyType[itemType]);
            return itemFunc(item, conversationContext, itemDataObj);
        }
    }
}