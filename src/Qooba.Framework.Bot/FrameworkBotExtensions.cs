using Qooba.Framework.Bot.Abstractions;
using System;
using Qooba.Framework.Abstractions;
using System.Reflection;
using System.Linq;
using Qooba.Framework.Bot.Abstractions.Models;
using Qooba.Framework.Bot.Attributes;
using Qooba.Framework.Bot.Form;
using System.Collections.Concurrent;
using Qooba.Framework.Bot.Abstractions.Form;

namespace Qooba.Framework.Bot
{
    public static class FrameworkBotExtensions
    {
        private static ConcurrentBag<Type> registeredCompletionActions = new ConcurrentBag<Type>();

        private static ConcurrentBag<Type> registeredValidators = new ConcurrentBag<Type>();

        private static ConcurrentBag<Type> registeredActions = new ConcurrentBag<Type>();

        public static IFramework AddBotForm<TModel>(this IFramework framework)
            where TModel : class
        {
            var routes = typeof(TModel).GetTypeInfo().GetCustomAttributes().Select(x => (x as RouteAttribute)?.Route).Where(x => x != null).ToArray();
            return AddBotForm<TModel>(framework, routes);
        }

        public static IFramework AddBotForm<TModel>(this IFramework framework, string[] routes)
            where TModel : class
        {
            var modelTypeInfo = typeof(TModel).GetTypeInfo();
            var completionActions = modelTypeInfo.GetCustomAttributes().Select(x => x as CompletionActionAttribute).Where(x => x?.Type != null && !registeredCompletionActions.Contains(x.Type));
            foreach (var completionAction in completionActions)
            {
                if (!registeredCompletionActions.Contains(completionAction.Type))
                {
                    framework.AddService(s => s.Service<IFormReplyCompletionAction>().As(completionAction.Type).Keyed(completionAction.TypeKey));
                    registeredCompletionActions.Add(completionAction.Type);
                }
            }

            var propertiesAttributes = modelTypeInfo.GetProperties().SelectMany(x => x.GetCustomAttributes());
            var validators = propertiesAttributes.Select(x => x as PropertyValidatorAttribute).Where(x => x?.Type != null && !registeredValidators.Contains(x.Type));
            foreach (var validator in validators)
            {
                if (!registeredValidators.Contains(validator.Type))
                {
                    framework.AddService(s => s.Service<IFormReplyPropertyValidator>().As(validator.Type).Keyed(validator.TypeKey));
                    registeredValidators.Add(validator.Type);
                }
            }

            var actions = propertiesAttributes.Select(x => x as PropertyReplyAttribute).Where(x => x?.Type != null && !registeredActions.Contains(x.Type));
            foreach (var action in actions)
            {
                if (!registeredActions.Contains(action.Type))
                {
                    AddBotAction(framework, action.Type, action.TypeKey);
                    registeredActions.Add(action.Type);
                }
            }

            return AddBotAction<FormReplyAction<TModel>>(framework, routes);
        }

        public static IFramework AddBotDefaultAction<TReplyAction>(this IFramework framework)
            where TReplyAction : class, IReplyAction => AddBotDefaultAction(framework, typeof(TReplyAction));

        public static IFramework AddBotGlobalCommandAction<TReplyAction>(this IFramework framework)
            where TReplyAction : class, IReplyAction => AddBotGlobalCommandAction(framework, typeof(TReplyAction));

        public static IFramework AddBotAction<TReplyAction>(this IFramework framework)
            where TReplyAction : class, IReplyAction => AddBotAction<TReplyAction>(framework, typeof(TReplyAction));


        public static IFramework AddBotAction<TReplyAction>(this IFramework framework, string[] routes)
            where TReplyAction : class, IReplyAction => AddBotAction(framework, typeof(TReplyAction), routes);


        public static IFramework AddBotAction<TReplyAction>(this IFramework framework, string replyType)
        where TReplyAction : class, IReplyAction => AddBotAction(framework, typeof(TReplyAction), replyType);

        private static IFramework AddBotDefaultAction(this IFramework framework, Type replyActionType)
        {
            return AddBotAction(framework, replyActionType, new[] { "#default" }, true, false);
        }

        private static IFramework AddBotGlobalCommandAction(this IFramework framework, Type replyActionType)
        {
            var routes = replyActionType.GetTypeInfo().GetCustomAttributes().Select(x => (x as RouteAttribute)?.Route).Where(x => x != null).ToArray();
            return AddBotAction(framework, replyActionType, routes, false, true);
        }

        private static IFramework AddBotAction<TReplyAction>(this IFramework framework, Type replyActionType)
            where TReplyAction : class, IReplyAction
        {
            var routes = replyActionType.GetTypeInfo().GetCustomAttributes().Select(x => (x as RouteAttribute)?.Route).Where(x => x != null).ToArray();
            return AddBotAction<TReplyAction>(framework, routes);
        }

        private static IFramework AddBotAction(this IFramework framework, Type replyActionType, string[] routes)
        {
            return AddBotAction(framework, replyActionType, routes, false, false);
        }

        private static IFramework AddBotAction(this IFramework framework, Type replyActionType, string[] routes, bool isDefault, bool isGlobalCommand)
        {
            var replyId = replyActionType.FullName;
            var replyItem = new ReplyItem
            {
                Routes = routes,
                ReplyId = replyId,
                ReplyType = replyId,
                IsDefault = isDefault,
                IsGlobalCommand = isGlobalCommand
            };

            framework.AddService(s => s.Service(typeof(ReplyItem)).As(replyItem).Keyed(replyId).Lifetime(Lifetime.Singleton));
            return AddBotAction(framework, replyActionType, replyId);
        }

        private static IFramework AddBotAction(this IFramework framework, Type replyActionType, string replyType)
        {
            Type type = null;
            foreach (var i in replyActionType.GetTypeInfo().GetInterfaces())
            {
                if (i.GetTypeInfo().IsGenericType && i.GetGenericTypeDefinition().Name.Contains("IReplyAction"))
                {
                    type = i.GetGenericArguments().FirstOrDefault();
                    break;
                }
            }

            var a = typeof(ReplyActionBuilder<,>);
            var args = new[] { type, replyActionType };
            var raType = a.MakeGenericType(args);

            return framework
                .AddTransientService(replyActionType)
                .AddService(s => s.Service<IReplyBuilder>().As(raType).Keyed(replyType));
        }
    }
}
