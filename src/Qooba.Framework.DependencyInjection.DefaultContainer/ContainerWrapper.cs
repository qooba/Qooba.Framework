using Qooba.Framework.DependencyInjection.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Qooba.Framework.DependencyInjection.DefaultContainer
{
    public class ContainerWrapper : IContainer
    {
        private readonly IServiceCollection services;
        public ContainerWrapper(IServiceCollection services)
        {
            this.services = services;
        }

        public static void SetupContainer(IServiceCollection services)
        {
            var container = new ContainerWrapper(services);
            container.RegisterInstance<IContainer>(container);
            ContainerManager.SetContainer(container);
        }

        public object BuildUp(Type t, object existing)
        {
            throw new NotImplementedException();
        }

        public T BuildUp<T>(T existing)
        {
            throw new NotImplementedException();
        }

        public T BuildUp<T>(T existing, string name)
        {
            throw new NotImplementedException();
        }

        public bool IsRegistered(Type typeToCheck)
        {
            throw new NotImplementedException();
        }

        public bool IsRegistered(Type typeToCheck, string nameToCheck)
        {
            throw new NotImplementedException();
        }

        public bool IsRegistered<T>()
        {
            throw new NotImplementedException();
        }

        public bool IsRegistered<T>(string nameToCheck)
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

        public IContainer RegisterInstance(Type t, string name, object instance)
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

        public IContainer RegisterInstance<TInterface>(string name, TInterface instance)
        {
            throw new NotImplementedException();
        }

        public IContainer RegisterInstance<TInterface>(string name, TInterface instance, Lifetime lifetime)
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

        public IContainer RegisterType(Type t, string name)
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

        public IContainer RegisterType(Type from, Type to, string name)
        {
            throw new NotImplementedException();
        }

        public IContainer RegisterType(Type from, Type to, Lifetime lifetime)
        {
            throw new NotImplementedException();
        }

        public IContainer RegisterType(Type t, string name, Lifetime lifetime)
        {
            throw new NotImplementedException();
        }

        public IContainer RegisterType<T>()
        {
            throw new NotImplementedException();
        }

        public IContainer RegisterType<T>(string name)
        {
            throw new NotImplementedException();
        }

        public IContainer RegisterType<T>(Lifetime lifetime)
        {
            throw new NotImplementedException();
        }

        public IContainer RegisterType<T>(string name, Lifetime lifetime)
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

        public IContainer RegisterType<TFrom, TTo>(string name) where TTo : class, TFrom
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

        public IContainer RegisterType<TFrom, TTo>(string name, Lifetime lifetime) where TTo : TFrom
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

        public T Resolve<T>(string name)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> ResolveAll<T>(string name)
        {
            return this.services.BuildServiceProvider().GetServices<T>();
        }

        public IEnumerable<T> ResolveAll<T>()
        {
            throw new NotImplementedException();
        }
    }
}
