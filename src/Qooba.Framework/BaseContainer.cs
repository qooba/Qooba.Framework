using System;
using System.Collections.Generic;
using Qooba.Framework.Abstractions;
using System.Linq;

namespace Qooba.Framework
{
    public abstract class BaseContainer : IContainer
    {
        public IServiceManager AddService(Func<IServiceDescriptor, IServiceDescriptor> serviceDescriptorFactory)
        {
            ServiceDescriptor serviceDescriptor = (ServiceDescriptor)serviceDescriptorFactory(new ServiceDescriptor());

            if (serviceDescriptor.ServiceType == null)
            {
                throw new InvalidOperationException("Upps ... service type not defined.");
            }

            if (serviceDescriptor.ImplementationInstance != null)
            {
                this.RegisterInstance(serviceDescriptor.Key, serviceDescriptor.ServiceType, serviceDescriptor.ImplementationInstance);
            }
            else if (serviceDescriptor.ImplementationType != null)
            {
                this.RegisterType(serviceDescriptor.Key, serviceDescriptor.ServiceType, serviceDescriptor.ImplementationType, serviceDescriptor.LifetimeType);
            }
            else if (serviceDescriptor.ImplementationFactory != null)
            {
                this.RegisterFactory(serviceDescriptor.Key, serviceDescriptor.ServiceType, serviceDescriptor.ImplementationFactory, serviceDescriptor.LifetimeType);
            }
            else
            {
                throw new InvalidOperationException("Upps ... implementation type not defined.");
            }

            return this;
        }

        public TService GetService<TService>() where TService : class => GetService(typeof(TService)) as TService;

        public object GetService(Type serviceType) => this.Resolve(null, serviceType);

        public TService GetService<TService>(object key) where TService : class => this.Resolve(key, typeof(TService)) as TService;

        public object GetService(object key, Type serviceType) => this.Resolve(key, serviceType);

        public IEnumerable<TService> GetServices<TService>() where TService : class => this.ResolveAll(typeof(TService)).Cast<TService>();

        public IEnumerable<object> GetServices(Type serviceType) => this.ResolveAll(serviceType);

        public abstract bool IsRegistered(Type typeToCheck, object keyToCheck);

        public abstract IContainer RegisterFactory(object key, Type from, Func<IContainer, object> implementationFactory, Lifetime lifetime);

        public abstract IContainer RegisterInstance(object key, Type from, object instance);

        public abstract IContainer RegisterType(object key, Type from, Type to, Lifetime lifetime);

        public abstract object Resolve(object key, Type from);

        public abstract IEnumerable<object> ResolveAll(Type from);
    }
}
