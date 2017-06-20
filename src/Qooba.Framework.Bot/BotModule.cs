using Qooba.Framework.Abstractions;
using Qooba.Framework.Bot.Abstractions;
using Qooba.Framework.Bot.Abstractions.Form;
using Qooba.Framework.Bot.Abstractions.Models;
using Qooba.Framework.Bot.Dispatch;
using Qooba.Framework.Bot.Form;
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
            framework.AddService(s => s.Service<IReplyBuilder>().As<EnumReplyBuilder>().Keyed("enum"));
            framework.AddService(s => s.Service<IReplyBuilder>().As<LocationReplyBuilder>().Keyed("location"));
            framework.AddService(s => s.Service<IReplyBuilder>().As<FormReplyBuilder>().Keyed("form"));
            framework.AddService(s => s.Service<IReplyBuilder>().As<HttpReplyBuilder>().Keyed("http"));
            framework.AddService(s => s.Service<IReplyBuilder>().As<ButtonTemplateReplyBuilder>().Keyed("buttonTemplate"));
            framework.AddService(s => s.Service<IReplyBuilder>().As<CarouselReplyBuilder>().Keyed("carousel"));
            framework.AddService(s => s.Service<IReplyBuilder>().As<ListReplyBuilder>().Keyed("list"));
            framework.AddService(s => s.Service<IReplyBuilder>().As<CallReplyBuilder>().Keyed("call"));
            
            framework.AddService(s => s.Service<IReplyBuilder<RawReplyMessage>>().As<RawReplyBuilder>());
            framework.AddService(s => s.Service<IReplyBuilder<TextReplyMessage>>().As<TextReplyBuilder>());
            framework.AddService(s => s.Service<IReplyBuilder<ImageReplyMessage>>().As<ImageReplyBuilder>());
            framework.AddService(s => s.Service<IReplyBuilder<VideoReplyMessage>>().As<VideoReplyBuilder>());
            framework.AddService(s => s.Service<IReplyBuilder<AudioReplyMessage>>().As<AudioReplyBuilder>());
            framework.AddService(s => s.Service<IReplyBuilder<FileReplyMessage>>().As<FileReplyBuilder>());
            framework.AddService(s => s.Service<IReplyBuilder<EnumReplyMessage>>().As<EnumReplyBuilder>());
            framework.AddService(s => s.Service<IReplyBuilder<LocationReplyMessage>>().As<LocationReplyBuilder>());
            framework.AddService(s => s.Service<IReplyBuilder<FormReplyMessage>>().As<FormReplyBuilder>());
            framework.AddService(s => s.Service<IReplyBuilder<HttpReplyMessage>>().As<HttpReplyBuilder>());
            framework.AddService(s => s.Service<IReplyBuilder<ButtonTemplateReplyMessage>>().As<ButtonTemplateReplyBuilder>());
            framework.AddService(s => s.Service<IReplyBuilder<CarouselReplyMessage>>().As<CarouselReplyBuilder>());
            framework.AddService(s => s.Service<IReplyBuilder<ListReplyMessage>>().As<ListReplyBuilder>());
            framework.AddService(s => s.Service<IReplyBuilder<CallReplyMessage>>().As<CallReplyBuilder>());

            framework.AddService(s => s.Service<IFormReplyCompletionAction>().As<TextFormReplyCompletionAction>().Keyed("text"));
            framework.AddService(s => s.Service<IFormReplyCompletionAction>().As<HttpFormReplyCompletionAction>().Keyed("http"));

            framework.AddTransientService<IHandlerManager, HandlerManager>();
            framework.AddSingletonService<IReplyConfiguration, ReplyManager>();
            framework.AddSingletonService<IReplyFactory, ReplyFactory>();
            framework.AddSingletonService<IRoutingConfiguration, ReplyManager>();
            framework.AddSingletonService<IGenericExpressionFactory, GenericExpressionFactory>();

            framework.AddService(s => s.Service<IRouter>().As<DefaultRouter>().Keyed(nameof(DefaultRouter)));
            framework.AddService(s => s.Service<IRouter>().As<RegexRouter>().Keyed(nameof(RegexRouter)).Lifetime(Lifetime.Singleton));

            framework.AddTransientService<IMessageQueue, MemoryMessageQueue>();
            framework.AddTransientService<IBot, QBot>();
            framework.AddSingletonService<IBotConfig, BotConfig>();
            framework.AddTransientService<Func<object, IReplyBuilder>>(s => (Func<object, IReplyBuilder>)((key) => (IReplyBuilder)s.GetService(key, typeof(IReplyBuilder))));
            framework.AddTransientService<Func<object, IDispatcher>>(s => (Func<object, IDispatcher>)((key) => (IDispatcher)s.GetService(key, typeof(IDispatcher))));
            framework.AddTransientService<Func<object, IUserManager>>(s => (Func<object, IUserManager>)((key) => (IUserManager)s.GetService(key, typeof(IUserManager))));
            framework.AddTransientService<Func<object, IFormReplyCompletionAction>>(s => (Func<object, IFormReplyCompletionAction>)((key) => (IFormReplyCompletionAction)s.GetService(key, typeof(IFormReplyCompletionAction))));
            framework.AddTransientService<Func<object, IFormReplyPropertyActiveConstraint>>(s => (Func<object, IFormReplyPropertyActiveConstraint>)((key) => (IFormReplyPropertyActiveConstraint)s.GetService(key, typeof(IFormReplyPropertyActiveConstraint))));
            framework.AddTransientService<Func<object, IFormReplyPropertyValidator>>(s => (Func<object, IFormReplyPropertyValidator>)((key) => (IFormReplyPropertyValidator)s.GetService(key, typeof(IFormReplyPropertyValidator))));
        }
    }
}
