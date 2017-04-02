using Qooba.Framework.Abstractions;
using Qooba.Framework.Bot.Abstractions;
using Qooba.Framework.Bot.Abstractions.Models;
using Qooba.Framework.Bot.Context;
using Qooba.Framework.Bot.Dispatch;
using Qooba.Framework.Bot.Handlers;
using Qooba.Framework.Bot.Queue;
using Qooba.Framework.Bot.Routing;

namespace Qooba.Framework.Bot
{
    public class BotModule : IModule
    {
        public virtual string Name => "BotModule";

        public int Priority => 10;

        public void Bootstrapp(IFramework framework)
        {
            framework.AddService(s => s.Service<IConnector>().As<MessangerConnector>().Keyed(ConnectorType.Messanger));
            framework.AddTransientService<IMessangerSecurity, MessangerSecurity>();
            framework.AddTransientService<IStateManager, StateManager>();
            framework.AddService(s => s.Service<IDispatcher>().As<MessangerDispatcher>().Keyed(ConnectorType.Messanger));

            framework.AddService(s => s.Service<IHandler>().As<ContextHandler>().Keyed(HandlerType.Context));
            framework.AddService(s => s.Service<IHandler>().As<RouteHandler>().Keyed(HandlerType.Route));
            framework.AddService(s => s.Service<IHandler>().As<ReplyHandler>().Keyed(HandlerType.Reply));
            framework.AddService(s => s.Service<IHandler>().As<DispatchHandler>().Keyed(HandlerType.Dispatch));
            framework.AddService(s => s.Service<IHandler>().As<ContextKeeperHandler>().Keyed(HandlerType.ContextKeeper));

            framework.AddTransientService<IHandlerManager, HandlerManager>();
            framework.AddSingletonService<IReplyManager, ReplyManager>();
            framework.AddTransientService<IRouter, Router>();
            framework.AddTransientService<IMessageQueue, MemoryMessageQueue>();
            framework.AddTransientService<IBot, QBot>();
        }
    }
}
