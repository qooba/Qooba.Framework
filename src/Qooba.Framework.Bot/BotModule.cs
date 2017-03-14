using Qooba.Framework.Abstractions;
using Qooba.Framework.Bot.Abstractions;
using Qooba.Framework.Bot.Abstractions.Models;
using Qooba.Framework.Bot.Context;
using Qooba.Framework.Bot.Dispatch;
using Qooba.Framework.Bot.Handlers;
using Qooba.Framework.Bot.Queue;
using Qooba.Framework.Bot.Routing;

namespace Qooba.Framework.Bot.Builder
{
    public class BotModule : IModule
    {
        public virtual string Name => "BotModule";

        public int Priority => 10;

        public void Bootstrapp(IContainer container)
        {
            container.RegisterType<IConnector, MessangerConnector>(ConnectorType.Messanger);
            container.RegisterType<IMessangerSecurity, MessangerSecurity>();
            container.RegisterType<IStateManager, StateManager>();
            container.RegisterType<IDispatcher, MessangerDispatcher>(ConnectorType.Messanger);

            container.RegisterType<IHandler, ContextHandler>(HandlerType.Context);
            container.RegisterType<IHandler, RouteHandler>(HandlerType.Route);
            container.RegisterType<IHandler, ReplyHandler>(HandlerType.Reply);
            container.RegisterType<IHandler, DispatchHandler>(HandlerType.Dispatch);
            container.RegisterType<IHandler, ContextKeeperHandler>(HandlerType.ContextKeeper);

            container.RegisterType<IHandlerManager, HandlerManager>();
            container.RegisterType<IReplyManager, ReplyManager>();
            container.RegisterType<IRouter, Router>();

            container.RegisterType(typeof(IMessageQueue<>), typeof(MemoryMessageQueue<>));

            container.RegisterType<IBot, QBot>();
        }
    }
}
