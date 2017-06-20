using Qooba.Framework.Bot.Abstractions;
using System;
using Qooba.Framework.Abstractions;
using System.Reflection;
using System.Linq;

namespace Qooba.Framework.Bot
{
    public static class FrameworkBotExtensions
    {
        public static IFramework AddBotAction<TReplyAction>(this IFramework framework, string replyType)
            where TReplyAction : class, IReplyAction
        {
            Type type = null;
            foreach (var i in typeof(TReplyAction).GetTypeInfo().GetInterfaces())
            {
                if (i.GetTypeInfo().IsGenericType && i.GetGenericTypeDefinition().Name.Contains("IReplyAction"))
                {
                    type = i.GetGenericArguments().FirstOrDefault();
                    break;
                }
            }

            var a = typeof(ReplyActionBuilder<,>);
            var args = new[] { type, typeof(TReplyAction) };
            var raType = a.MakeGenericType(args);

            return framework
                .AddTransientService<TReplyAction, TReplyAction>()
                .AddService(s => s.Service<IReplyBuilder>().As(raType).Keyed(replyType));
        }
    }
}
