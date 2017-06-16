using Qooba.Framework.Bot.Abstractions;
using System;
using Qooba.Framework.Abstractions;
using Qooba.Framework.Bot.Reply;

namespace Qooba.Framework.Bot
{
    public static class FrameworkBotExtensions
    {
        public static IFramework AddBotAction<T>(this IFramework framework, Func<IReplyAction<T>> replyActionFunc)
            where T : class
        {
            var replyAction = replyActionFunc();
            framework.AddService(s => s.Service<IReplyBuilder>().As(sp => new ReplyActionBuilder<T>((IReplyBuilder<T>)sp.GetService(typeof(IReplyBuilder<T>))) { Action = replyActionFunc }).Keyed(replyAction.GetType().FullName));
            return framework;
        }

    }
}
