using Autofac;
using Autofac.Builder;
using Qooba.Framework.Abstractions;
using Qooba.Framework.DependencyInjection.Abstractions;
using System;
using System.Collections.Generic;

namespace Qooba.Framework.DependencyInjection.AutofacContainer
{
    public class AutofacContainerWrapper : Abstractions.IContainer
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

        public object BuildUp(Type t, object existing)
        {
            throw new NotImplementedException();
        }

        public T BuildUp<T>(T existing)
        {
            return container.Value.InjectProperties(existing);
        }

        public T BuildUp<T>(T existing, object key)
        {
            throw new NotImplementedException();
        }

        public bool IsRegistered(Type typeToCheck)
        {
            return container.Value.IsRegistered(typeToCheck);
        }

        public bool IsRegistered(Type typeToCheck, object keyToCheck)
        {
            return container.Value.IsRegisteredWithKey(keyToCheck, typeToCheck);
        }

        public bool IsRegistered<T>()
            where T : class
        {
            return container.Value.IsRegistered<T>();
        }

        public bool IsRegistered<T>(object keyToCheck)
            where T : class
        {
            return container.Value.IsRegisteredWithKey<T>(keyToCheck);
        }

        public void Populate(object services)
        {
            throw new NotImplementedException();
        }

        public Framework.Abstractions.IContainer RegisterInstance(Type t, object instance)
        {
            BuilderAction(b => b.RegisterInstance(instance).As(t));
            return this;
        }

        public Framework.Abstractions.IContainer RegisterInstance(Type t, object key, object instance)
        {
            BuilderAction(b => b.RegisterInstance(instance).Keyed(key, t));
            return this;
        }

        public Framework.Abstractions.IContainer RegisterInstance(Type t, object instance, Lifetime lifetime)
        {
            AddLifetime(b => b.RegisterInstance(instance).As(t), lifetime);
            return this;
        }

        public Framework.Abstractions.IContainer RegisterInstance<TInterface>(TInterface instance)
        {
            this.RegisterInstance(t: typeof(TInterface), instance: instance);
            return this;
        }

        public Framework.Abstractions.IContainer RegisterInstance<TInterface>(TInterface instance, Lifetime lifetime)
        {
            this.RegisterInstance(t: typeof(TInterface), instance: instance, lifetime: lifetime);
            return this;
        }

        public Framework.Abstractions.IContainer RegisterInstance<TInterface>(object key, TInterface instance)
        {
            this.RegisterInstance(typeof(TInterface), key, instance);
            return this;
        }

        public Framework.Abstractions.IContainer RegisterInstance<TInterface>(object key, TInterface instance, Lifetime lifetime)
        {
            AddLifetime(b => b.RegisterInstance(instance as object).Keyed(key, typeof(TInterface)), lifetime);
            return this;
        }

        public Framework.Abstractions.IContainer RegisterType(Type t)
        {
            return this.RegisterType(t, Lifetime.Transistent);
        }

        public Framework.Abstractions.IContainer RegisterType(Type from, Type to)
        {
            BuilderAction(b => b.RegisterType(to).As(from));
            return this;
        }

        public Framework.Abstractions.IContainer RegisterType(Type t, object key)
        {
            BuilderAction(b => b.RegisterType(t).Keyed(key, t));
            return this;
        }

        public Framework.Abstractions.IContainer RegisterType(Type t, Lifetime lifetime)
        {
            AddLifetime(b => b.RegisterType(t), lifetime);
            return this;
        }

        public Framework.Abstractions.IContainer RegisterType(Type from, Type to, object key)
        {
            BuilderAction(b => b.RegisterType(from).Keyed(key, to));
            return this;
        }

        public Framework.Abstractions.IContainer RegisterType(Type from, Type to, Lifetime lifetime)
        {
            AddLifetime(b => b.RegisterType(from).As(to), lifetime);
            return this;
        }

        public Framework.Abstractions.IContainer RegisterType(Type t, object key, Lifetime lifetime)
        {
            AddLifetime(b => b.RegisterType(t).Keyed(key, t), lifetime);
            return this;
        }

        public Framework.Abstractions.IContainer RegisterType<T>()
            where T : class
        {
            BuilderAction(b => b.RegisterType<T>());
            return this;
        }

        public Framework.Abstractions.IContainer RegisterType<T>(object key)
            where T : class
        {
            BuilderAction(b => b.RegisterType<T>().Keyed<T>(key));
            return this;
        }

        public Framework.Abstractions.IContainer RegisterType<T>(Lifetime lifetime)
            where T : class
        {
            AddLifetime(b => b.RegisterType<T>(), lifetime);
            return this;
        }

        public Framework.Abstractions.IContainer RegisterType<T>(object key, Lifetime lifetime)
            where T : class
        {
            AddLifetime(b => b.RegisterType<T>().Keyed<T>(key), lifetime);
            return this;
        }

        public Framework.Abstractions.IContainer RegisterType<TFrom, TTo>()
            where TTo : class, TFrom
            where TFrom : class
        {
            BuilderAction(b => b.RegisterType<TFrom>().As<TTo>());
            return this;
        }

        public Framework.Abstractions.IContainer RegisterType<TFrom, TTo>(object key) where TTo : class, TFrom
        {
            BuilderAction(b => b.RegisterType<TFrom>().Keyed<TTo>(key));
            return this;
        }

        public Framework.Abstractions.IContainer RegisterType<TFrom, TTo>(Lifetime lifetime)
            where TTo : class, TFrom
            where TFrom : class
        {
            AddLifetime(b => b.RegisterType<TFrom>().As<TTo>(), lifetime);
            return this;
        }

        public Framework.Abstractions.IContainer RegisterType<TFrom, TTo>(object key, Lifetime lifetime) where TTo : TFrom
        {
            AddLifetime(b => b.RegisterType<TFrom>().Keyed<TTo>(key), lifetime);
            return this;
        }

        public object Resolve(Type t)
        {
            return container.Value.Resolve(t);
        }

        public IEnumerable<object> ResolveAll(Type t)
        {
            //TODO: FIX !!!
            return new[] { container.Value.Resolve(t) };
        }

        public T Resolve<T>() where T : class
        {
            return container.Value.Resolve<T>();
        }

        public T Resolve<T>(object key)
            where T : class
        {
            return container.Value.ResolveKeyed<T>(key);
        }

        public IEnumerable<T> ResolveAll<T>(object key)
        {
            return container.Value.ResolveKeyed<IEnumerable<T>>(key);
        }

        public IEnumerable<T> ResolveAll<T>()
            where T : class
        {
            return container.Value.Resolve<IEnumerable<T>>();
        }

        public Framework.Abstractions.IContainer RegisterType<T>(Func<Framework.Abstractions.IContainer, T> implementationFactory) where T : class
        {
            BuilderAction(b => b.Register<T>(c =>
            {
                var ctx = c.Resolve<IComponentContext>();
                var container = new AutofacContainerWrapper(ctx);
                return implementationFactory(container);
            }));
            return this;
        }

        public Framework.Abstractions.IContainer RegisterType<T>(Func<Framework.Abstractions.IContainer, T> implementationFactory, Lifetime lifetime) where T : class
        {
            AddLifetime(b => b.Register<T>(c =>
            {
                var ctx = c.Resolve<IComponentContext>();
                var container = new AutofacContainerWrapper(ctx);
                return implementationFactory(container);
            }), lifetime);
            return this;
        }

        public Framework.Abstractions.IContainer RegisterType<T>(object key, Func<Framework.Abstractions.IContainer, T> implementationFactory) where T : class
        {
            BuilderAction(b => b.Register<T>(c =>
            {
                var ctx = c.Resolve<IComponentContext>();
                var container = new AutofacContainerWrapper(ctx);
                return implementationFactory(container);
            }).Keyed<T>(key));
            return this;
        }

        public Framework.Abstractions.IContainer RegisterType<T>(object key, Func<Framework.Abstractions.IContainer, T> implementationFactory, Lifetime lifetime) where T : class
        {
            AddLifetime(b => b.Register<T>(c =>
            {
                var ctx = c.Resolve<IComponentContext>();
                var container = new AutofacContainerWrapper(ctx);
                return implementationFactory(container);
            }).Keyed<T>(key), lifetime);
            return this;
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
            else if (lifetime == Lifetime.PerRequest)
            {
                BuilderAction(b => registrationBuilderFunc(b).InstancePerRequest());
            }
            else if (lifetime == Lifetime.PerThread)
            {
                BuilderAction(b => registrationBuilderFunc(b).InstancePerLifetimeScope());
            }
            else
            {
                BuilderAction(b => registrationBuilderFunc(b));
            }
        }
    }
}
