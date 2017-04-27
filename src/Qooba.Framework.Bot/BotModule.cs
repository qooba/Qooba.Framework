using Qooba.Framework.Abstractions;
using Qooba.Framework.Bot.Abstractions;
using Qooba.Framework.Bot.Abstractions.Models;
using Qooba.Framework.Bot.Dispatch;
using Qooba.Framework.Bot.Handlers;
using Qooba.Framework.Bot.Queue;
using Qooba.Framework.Bot.Routing;
using Qooba.Framework.Bot.Users;
using System;

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
            framework.AddService(s => s.Service<IDispatcher>().As<MessangerDispatcher>().Keyed(ConnectorType.Messanger));
            framework.AddService(s => s.Service<IUserManager>().As<MessangerUserManager>().Keyed(ConnectorType.Messanger));

            framework.AddService(s => s.Service<IHandler>().As<ContextHandler>().Keyed(HandlerType.Context));
            framework.AddService(s => s.Service<IHandler>().As<UserDataHandler>().Keyed(HandlerType.UserData));
            framework.AddService(s => s.Service<IHandler>().As<RouteHandler>().Keyed(HandlerType.Route));
            framework.AddService(s => s.Service<IHandler>().As<ReplyHandler>().Keyed(HandlerType.Reply));
            framework.AddService(s => s.Service<IHandler>().As<DispatchHandler>().Keyed(HandlerType.Dispatch));
            framework.AddService(s => s.Service<IHandler>().As<ContextKeeperHandler>().Keyed(HandlerType.ContextKeeper));

            framework.AddService(s => s.Service<IUserManager>().As<MessangerUserManager>().Keyed(ConnectorType.Messanger));

            framework.AddService(s => s.Service<IReplyBuilder>().As<RawReplyBuilder>().Keyed("raw"));
            framework.AddService(s => s.Service<IReplyBuilder>().As<TextReplyBuilder>().Keyed("text"));
            framework.AddService(s => s.Service<IReplyBuilder>().As<ImageReplyBuilder>().Keyed("image"));
            framework.AddService(s => s.Service<IReplyBuilder>().As<VideoReplyBuilder>().Keyed("video"));
            framework.AddService(s => s.Service<IReplyBuilder>().As<AudioReplyBuilder>().Keyed("audio"));
            framework.AddService(s => s.Service<IReplyBuilder>().As<FileReplyBuilder>().Keyed("file"));
            framework.AddService(s => s.Service<IReplyBuilder>().As<PostbackButtonTemplateReplyBuilder>().Keyed("postbackButtonTemplate"));
            framework.AddService(s => s.Service<IReplyBuilder>().As<PostbackCarouselReplyBuilder>().Keyed("postbackCarousel"));

            framework.AddTransientService<IHandlerManager, HandlerManager>();
            framework.AddSingletonService<IReplyConfiguration, ReplyManager>();
            framework.AddSingletonService<IReplyFactory, ReplyFactory>();
            framework.AddSingletonService<IRoutingConfiguration, ReplyManager>();

            framework.AddService(s => s.Service<IRouter>().As<DefaultRouter>().Keyed(nameof(DefaultRouter)));
            framework.AddService(s => s.Service<IRouter>().As<RegexRouter>().Keyed(nameof(RegexRouter)).Lifetime(Lifetime.Singleton));

            framework.AddTransientService<IMessageQueue, MemoryMessageQueue>();
            framework.AddTransientService<IBot, QBot>();
            framework.AddSingletonService<IBotConfig, BotConfig>();
            framework.AddTransientService<Func<object, IReplyBuilder>>(s => (Func<object, IReplyBuilder>)((key) => (IReplyBuilder)s.GetService(key, typeof(IReplyBuilder))));
            framework.AddTransientService<Func<object, IDispatcher>>(s => (Func<object, IDispatcher>)((key) => (IDispatcher)s.GetService(key, typeof(IDispatcher))));
            framework.AddTransientService<Func<object, IUserManager>>(s => (Func<object, IUserManager>)((key) => (IUserManager)s.GetService(key, typeof(IUserManager))));
        }
    }
}
