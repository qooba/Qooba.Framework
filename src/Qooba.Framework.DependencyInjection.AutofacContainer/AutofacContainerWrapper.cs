using Autofac;
using Autofac.Builder;
using Qooba.Framework.DependencyInjection.Abstractions;
using System;
using System.Collections.Generic;

namespace Qooba.Framework.DependencyInjection.AutofacContainer
{
    public class AutofacContainerWrapper : Abstractions.IContainer
    {
        private static Lazy<Autofac.IContainer> container;
        private readonly Autofac.ContainerBuilder builder;
        public AutofacContainerWrapper(Autofac.ContainerBuilder builder)
        {
            this.builder = builder;
            container = new Lazy<Autofac.IContainer>(() => this.builder.Build());
        }

        public object BuildUp(Type t, object existing)
        {
            throw new NotImplementedException();
        }

        public T BuildUp<T>(T existing)
        {
            return container.Value.InjectProperties(existing);
        }

        public T BuildUp<T>(T existing, string name)
        {
            throw new NotImplementedException();
        }

        public bool IsRegistered(Type typeToCheck)
        {
            return container.Value.IsRegistered(typeToCheck);
        }

        public bool IsRegistered(Type typeToCheck, string nameToCheck)
        {
            return container.Value.IsRegisteredWithKey(nameToCheck, typeToCheck);
        }

        public bool IsRegistered<T>()
        {
            return container.Value.IsRegistered<T>();
        }

        public bool IsRegistered<T>(string nameToCheck)
        {
            return container.Value.IsRegisteredWithKey<T>(nameToCheck);
        }

        public void Populate(object services)
        {
            throw new NotImplementedException();
        }

        public Abstractions.IContainer RegisterInstance(Type t, object instance)
        {
            BuilderAction(b => b.RegisterInstance(instance).As(t));
            return this;
        }

        public Abstractions.IContainer RegisterInstance(Type t, string name, object instance)
        {
            BuilderAction(b => b.RegisterInstance(instance).Keyed(name, t));
            return this;
        }

        public Abstractions.IContainer RegisterInstance(Type t, object instance, Lifetime lifetime)
        {
            AddLifetime(b => b.RegisterInstance(instance).As(t), lifetime);
            return this;
        }

        public Abstractions.IContainer RegisterInstance<TInterface>(TInterface instance)
        {
            this.RegisterInstance(typeof(TInterface), instance);
            return this;
        }

        public Abstractions.IContainer RegisterInstance<TInterface>(TInterface instance, Lifetime lifetime)
        {
            this.RegisterInstance(typeof(TInterface), instance, lifetime);
            return this;
        }

        public Abstractions.IContainer RegisterInstance<TInterface>(string name, TInterface instance)
        {
            this.RegisterInstance(typeof(TInterface), name, instance);
            return this;
        }

        public Abstractions.IContainer RegisterInstance<TInterface>(string name, TInterface instance, Lifetime lifetime)
        {
            AddLifetime(b => b.RegisterInstance(instance as object).Keyed(name, typeof(TInterface)), lifetime);
            return this;
        }

        public Abstractions.IContainer RegisterType(Type t)
        {
            return this.RegisterType(t, Lifetime.Transistent);
        }

        public Abstractions.IContainer RegisterType(Type from, Type to)
        {
            BuilderAction(b => b.RegisterType(to).As(from));
            return this;
        }

        public Abstractions.IContainer RegisterType(Type t, string name)
        {
            BuilderAction(b => b.RegisterType(t).Named(name, t));
            return this;
        }

        public Abstractions.IContainer RegisterType(Type t, Lifetime lifetime)
        {
            AddLifetime(b => b.RegisterType(t), lifetime);
            return this;
        }

        public Abstractions.IContainer RegisterType(Type from, Type to, string name)
        {
            BuilderAction(b => b.RegisterType(from).Keyed(name, to));
            return this;
        }

        public Abstractions.IContainer RegisterType(Type from, Type to, Lifetime lifetime)
        {
            AddLifetime(b => b.RegisterType(from).As(to), lifetime);
            return this;
        }

        public Abstractions.IContainer RegisterType(Type t, string name, Lifetime lifetime)
        {
            AddLifetime(b => b.RegisterType(t).Keyed(name, t), lifetime);
            return this;
        }

        public Abstractions.IContainer RegisterType<T>()
        {
            BuilderAction(b => b.RegisterType<T>());
            return this;
        }

        public Abstractions.IContainer RegisterType<T>(string name)
        {
            BuilderAction(b => b.RegisterType<T>().Keyed<T>(name));
            return this;
        }

        public Abstractions.IContainer RegisterType<T>(Lifetime lifetime)
        {
            AddLifetime(b => b.RegisterType<T>(), lifetime);
            return this;
        }

        public Abstractions.IContainer RegisterType<T>(string name, Lifetime lifetime)
        {
            AddLifetime(b => b.RegisterType<T>().Keyed<T>(name), lifetime);
            return this;
        }

        public Abstractions.IContainer RegisterType<TFrom, TTo>()
            where TTo : class, TFrom
            where TFrom : class
        {
            BuilderAction(b => b.RegisterType<TFrom>().As<TTo>());
            return this;
        }

        public Abstractions.IContainer RegisterType<TFrom, TTo>(string name) where TTo : class, TFrom
        {
            BuilderAction(b => b.RegisterType<TFrom>().Keyed<TTo>(name));
            return this;
        }

        public Abstractions.IContainer RegisterType<TFrom, TTo>(Lifetime lifetime)
            where TTo : class, TFrom
            where TFrom : class
        {
            AddLifetime(b => b.RegisterType<TFrom>().As<TTo>(), lifetime);
            return this;
        }

        public Abstractions.IContainer RegisterType<TFrom, TTo>(string name, Lifetime lifetime) where TTo : TFrom
        {
            AddLifetime(b => b.RegisterType<TFrom>().Keyed<TTo>(name), lifetime);
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

        public T Resolve<T>(string name)
        {
            return container.Value.ResolveKeyed<T>(name);
        }

        public IEnumerable<T> ResolveAll<T>(string name)
        {
            return container.Value.ResolveKeyed<IEnumerable<T>>(name);
        }

        public IEnumerable<T> ResolveAll<T>()
        {
            return container.Value.Resolve<IEnumerable<T>>();
        }

        private void BuilderAction(Action<Autofac.ContainerBuilder> builderAction)
        {
            if (container.IsValueCreated)
            {
                ContainerBuilder builder = new ContainerBuilder();
                builderAction(builder);
                builder.Update(container.Value);
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
