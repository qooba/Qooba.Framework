using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Qooba.Framework.Abstractions;
using Qooba.Framework.DependencyInjection.Abstractions;

namespace Qooba.Framework.DependencyInjection.DefaultContainer
{
    public class ContainerWrapper : IContainer
    {
        private readonly IServiceCollection services;
        public ContainerWrapper(IServiceCollection services)
        {
            this.services = services;
        }
        
        public bool IsRegistered(Type typeToCheck)
        {
            throw new NotImplementedException();
        }

        public bool IsRegistered(Type typeToCheck, object keyToCheck)
        {
            throw new NotImplementedException();
        }

        public bool IsRegistered<T>()
            where T : class
        {
            throw new NotImplementedException();
        }

        public bool IsRegistered<T>(object keyToCheck)
            where T : class
        {
            throw new NotImplementedException();
        }

        public void Populate(object services)
        {
            var servicesCollection = services as IServiceCollection;
            if (servicesCollection != null)
            {
                foreach (var service in servicesCollection)
                {
                    this.services.Add(service);
                }
            }
        }

        public IContainer RegisterInstance(Type t, object instance)
        {
            this.services.AddSingleton(t, instance);
            return this;
        }

        public IContainer RegisterInstance(Type t, object key, object instance)
        {
            throw new NotImplementedException();
        }

        public IContainer RegisterInstance(Type t, object instance, Lifetime lifetime)
        {
            throw new NotImplementedException();
        }

        public IContainer RegisterInstance<TInterface>(TInterface instance)
        {
            this.services.AddSingleton(typeof(TInterface), instance);
            return this;
        }

        public IContainer RegisterInstance<TInterface>(TInterface instance, Lifetime lifetime)
        {
            throw new NotImplementedException();
        }

        public IContainer RegisterInstance<TInterface>(object key, TInterface instance)
        {
            throw new NotImplementedException();
        }

        public IContainer RegisterInstance<TInterface>(object key, TInterface instance, Lifetime lifetime)
        {
            throw new NotImplementedException();
        }

        public IContainer RegisterType(Type t)
        {
            return this.RegisterType(t, Lifetime.Transistent);
        }

        public IContainer RegisterType(Type from, Type to)
        {
            this.services.AddTransient(from, to);
            return this;
        }

        public IContainer RegisterType(Type t, object key)
        {
            throw new NotImplementedException();
        }

        public IContainer RegisterType(Type t, Lifetime lifetime)
        {
            if (lifetime == Lifetime.Transistent)
            {
                this.services.AddTransient(t);
            }
            else
            {
                this.services.AddSingleton(t);
            }

            return this;
        }

        public IContainer RegisterType(Type from, Type to, object key)
        {
            throw new NotImplementedException();
        }

        public IContainer RegisterType(Type from, Type to, Lifetime lifetime)
        {
            throw new NotImplementedException();
        }

        public IContainer RegisterType(Type t, object key, Lifetime lifetime)
        {
            throw new NotImplementedException();
        }

        public IContainer RegisterType<T>()
            where T : class
        {
            throw new NotImplementedException();
        }

        public IContainer RegisterType<T>(object key)
            where T : class
        {
            throw new NotImplementedException();
        }

        public IContainer RegisterType<T>(Lifetime lifetime)
            where T : class
        {
            throw new NotImplementedException();
        }

        public IContainer RegisterType<T>(object key, Lifetime lifetime)
            where T : class
        {
            throw new NotImplementedException();
        }

        public IContainer RegisterType<TFrom, TTo>()
            where TTo : class, TFrom
            where TFrom : class
        {
            this.services.AddTransient<TFrom, TTo>();
            return this;
        }

        public IContainer RegisterType<TFrom, TTo>(object key) where TTo : class, TFrom
        {
            throw new NotImplementedException();
        }

        public IContainer RegisterType<TFrom, TTo>(Lifetime lifetime)
            where TTo : class, TFrom
            where TFrom : class
        {
            if (lifetime == Lifetime.Transistent)
            {
                this.services.AddTransient<TFrom, TTo>();
            }
            else
            {
                this.services.AddSingleton<TFrom, TTo>();
            }

            return this;
        }

        public IContainer RegisterType<TFrom, TTo>(object key, Lifetime lifetime) where TTo : TFrom
        {
            throw new NotImplementedException();
        }

        public object Resolve(Type t)
        {
            return this.services.BuildServiceProvider().GetService(t);
        }

        public IEnumerable<object> ResolveAll(Type t)
        {
            return this.services.BuildServiceProvider().GetServices(t);
        }

        public T Resolve<T>() where T : class
        {
            return this.services.BuildServiceProvider().GetService<T>();
        }

        public T Resolve<T>(object key)
            where T : class
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> ResolveAll<T>(object key)
        {
            return this.services.BuildServiceProvider().GetServices<T>();
        }

        public IEnumerable<T> ResolveAll<T>()
            where T : class
        {
            throw new NotImplementedException();
        }

        public IContainer RegisterType<T>(Func<IContainer, T> implementationFactory) where T : class
        {
            throw new NotImplementedException();
        }

        public IContainer RegisterType<T>(Func<IContainer, T> implementationFactory, Lifetime lifetime) where T : class
        {
            throw new NotImplementedException();
        }

        public IContainer RegisterType<T>(object key, Func<IContainer, T> implementationFactory) where T : class
        {
            throw new NotImplementedException();
        }

        public IContainer RegisterType<T>(object key, Func<IContainer, T> implementationFactory, Lifetime lifetime) where T : class
        {
            throw new NotImplementedException();
        }
    }
}
