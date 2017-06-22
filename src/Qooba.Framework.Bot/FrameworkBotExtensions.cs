using Qooba.Framework.Bot.Abstractions;
using System;
using Qooba.Framework.Abstractions;
using System.Reflection;
using System.Linq;
using Qooba.Framework.Bot.Abstractions.Models;
using Qooba.Framework.Bot.Attributes;
using Qooba.Framework.Bot.Form;

namespace Qooba.Framework.Bot
{
    public static class FrameworkBotExtensions
    {
        public static IFramework AddBotForm<TModel>(this IFramework framework)
            where TModel : class
        {
            var routes = typeof(TModel).GetTypeInfo().GetCustomAttributes().Select(x => (x as RouteAttribute)?.Route).Where(x => x != null).ToArray();
            return AddBotForm<TModel>(framework, routes);
        }

        public static IFramework AddBotForm<TModel>(this IFramework framework, string[] routes)
            where TModel : class
        {
            return AddBotAction<FormReplyAction<TModel>>(framework, routes);
        }

        public static IFramework AddBotDefaultAction<TReplyAction>(this IFramework framework)
            where TReplyAction : class, IReplyAction
        {
            return AddBotAction<TReplyAction>(framework, new[] { "#default" });
        }

        public static IFramework AddBotAction<TReplyAction>(this IFramework framework)
            where TReplyAction : class, IReplyAction
        {
            var routes = typeof(TReplyAction).GetTypeInfo().GetCustomAttributes().Select(x => (x as RouteAttribute)?.Route).Where(x => x != null).ToArray();
            return AddBotAction<TReplyAction>(framework, routes);
        }

        public static IFramework AddBotAction<TReplyAction>(this IFramework framework, string[] routes)
            where TReplyAction : class, IReplyAction
        {
            var replyId = typeof(TReplyAction).FullName;
            ((IFrameworkManager)framework).GetService<IReplyConfiguration>().AddConfiguration(new ReplyItem
            {
                Routes = routes,
                ReplyId = replyId,
                ReplyType = replyId
            });

            return AddBotAction<TReplyAction>(framework, replyId);
        }

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
