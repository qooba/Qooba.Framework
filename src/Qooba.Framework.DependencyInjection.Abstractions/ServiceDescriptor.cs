using System;
using Qooba.Framework.Abstractions;

namespace Qooba.Framework.DependencyInjection.Abstractions
{
    public class ServiceDescriptor : IServiceDescriptor
    {
        public Func<Framework.Abstractions.IServiceProvider, object> ImplementationFactory { get; private set; }

        public Type ImplementationType { get; private set; }

        public Type ServiceType { get; private set; }

        public object ImplementationInstance { get; private set; }

        public Lifetime LifetimeType { get; private set; }

        public IServiceLifetimeManager LifetimeManager { get; private set; }

        public object Key { get; private set; }

        public IServiceDescriptor As<TImplementation>() where TImplementation : class => this.As(typeof(TImplementation));

        public IServiceDescriptor As(Type implementationType)
        {
            this.ImplementationType = implementationType;
            return this;
        }

        public IServiceDescriptor As(Func<Framework.Abstractions.IServiceProvider, object> implementationFactory)
        {
            this.ImplementationFactory = implementationFactory;
            return this;
        }

        public IServiceDescriptor As<TImplementation>(Func<Framework.Abstractions.IServiceProvider, TImplementation> implementationFactory) where TImplementation : class => this.As(implementationFactory);

        public IServiceDescriptor As(object implementationInstance)
        {
            this.ImplementationInstance = implementationInstance;
            return this;
        }

        public IServiceDescriptor As<TImplementation>(TImplementation implementationInstance) where TImplementation : class => this.As((object)implementationInstance);

        public IServiceDescriptor Keyed(object key)
        {
            this.Key = key;
            return this;
        }

        public IServiceDescriptor Lifetime(Lifetime serivceLifetime)
        {
            this.LifetimeType = serivceLifetime;
            return this;
        }

        public IServiceDescriptor Lifetime(IServiceLifetimeManager serivceLifetimeManager)
        {
            this.LifetimeManager = serivceLifetimeManager;
            return this;
        }

        public IServiceDescriptor Service<TService>() where TService : class => this.Service(typeof(TService));

        public IServiceDescriptor Service(Type serviceType)
        {
            this.ServiceType = serviceType;
            return this;
        }
    }
}
