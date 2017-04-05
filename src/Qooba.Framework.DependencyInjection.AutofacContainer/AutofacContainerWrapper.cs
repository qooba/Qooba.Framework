using Autofac;
using Autofac.Builder;
using Qooba.Framework.Abstractions;
using Qooba.Framework.DependencyInjection.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Qooba.Framework.DependencyInjection.AutofacContainer
{
    public class AutofacContainerWrapper : BaseContainer
    {
        private static Lazy<IComponentContext> container;
        private readonly ContainerBuilder builder;

        public AutofacContainerWrapper(IComponentContext componentContex)
        {
            container = new Lazy<IComponentContext>(() => componentContex);
        }

        public AutofacContainerWrapper(Autofac.ContainerBuilder builder)
        {
            this.builder = builder;
            container = new Lazy<IComponentContext>(() => this.builder.Build());
        }

        public override bool IsRegistered(Type typeToCheck, object keyToCheck)
        {
            if (keyToCheck != null)
            {
                return container.Value.IsRegistered(typeToCheck);
            }
            else
            {
                return container.Value.IsRegisteredWithKey(keyToCheck, typeToCheck);
            }
        }

        public override Abstractions.IContainer RegisterInstance(object key, Type from, object instance)
        {
            if (key == null)
            {
                BuilderAction(b => b.RegisterInstance(instance).Keyed(key, from));
            }
            else
            {
                BuilderAction(b => b.RegisterInstance(instance));
            }

            return this;
        }

        public override Abstractions.IContainer RegisterType(object key, Type from, Type to, Lifetime lifetime)
        {
            if (key == null)
            {
                AddLifetime(b => b.RegisterType(from).As(to), lifetime);
            }
            else
            {
                AddLifetime(b => b.RegisterType(from).As(to).Keyed(key, to), lifetime);
            }

            return this;
        }

        public override Abstractions.IContainer RegisterFactory(object key, Type from, Func<Abstractions.IContainer, object> implementationFactory, Lifetime lifetime)
        {
            //TODO: Implement !!!
            //BuilderAction(b => b.Register(from, c =>
            //{
            //var ctx = c.Resolve<IComponentContext>();
            //var container = new AutofacContainerWrapper(ctx);
            //return implementationFactory(container);
            //}));
            return this;
        }

        public override object Resolve(object key, Type from)
        {
            if (key == null)
            {
                return container.Value.Resolve(from);
            }
            else
            {
                return container.Value.ResolveKeyed(key, from);
            }
        }

        public override IEnumerable<object> ResolveAll(Type from)
        {
            //TODO: FIX !!!
            return new[] { container.Value.Resolve(from) };
        }
        
        private void BuilderAction(Action<Autofac.ContainerBuilder> builderAction)
        {
            if (container.IsValueCreated)
            {
                ContainerBuilder builder = new ContainerBuilder();
                builderAction(builder);
                builder.Update(container.Value as Autofac.IContainer);
            }
            else
            {
                builderAction(this.builder);
            }
        }

        private void AddLifetime<TLimit, TActivatorData, TRegistrationStyle>(Func<Autofac.ContainerBuilder, IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle>> registrationBuilderFunc, Lifetime lifetime)
        {
            if (lifetime == Lifetime.Singleton)
            {
                BuilderAction(b => registrationBuilderFunc(b).SingleInstance());
            }
            else
            {
                BuilderAction(b => registrationBuilderFunc(b));
            }
        }
    }
}
