using System;

namespace Qooba.Framework.Abstractions
{
    public interface IServiceDescriptor
    {
        IServiceDescriptor Service<TService>() where TService : class;

        IServiceDescriptor Service(Type serviceType);

        IServiceDescriptor As<TImplementation>() where TImplementation : class;

        IServiceDescriptor As(Type implementationType);

        IServiceDescriptor As(Func<IServiceProvider, object> implementationFactory);

        IServiceDescriptor As<TImplementation>(Func<IServiceProvider, TImplementation> implementationFactory) where TImplementation : class;

        IServiceDescriptor As(object implementationInstance);

        IServiceDescriptor As<TImplementation>(TImplementation implementationInstance) where TImplementation : class;

        IServiceDescriptor Keyed(object key);

        IServiceDescriptor Lifetime(Lifetime serivceLifetime);

        IServiceDescriptor Lifetime(IServiceLifetimeManager serivceLifetimeManager);
    }
}
