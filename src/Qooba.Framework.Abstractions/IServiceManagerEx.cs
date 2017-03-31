using System;

namespace Qooba.Framework.Abstractions
{
    public interface IServiceManagerEx : IServiceManager
    {
        IServiceManager AddScopedService<TService>() where TService : class;

        IServiceManager AddScopedService(Type serviceType, Type implementationType);

        IServiceManager AddScopedService(Type serviceType, Func<IServiceProvider,object> implementationFactory);

        IServiceManager AddScopedService<TService,TImplementation>() where TService : class where TImplementation : class, TService;

        IServiceManager AddScopedService(Type serviceType);

        IServiceManager AddScopedService<TService>(Func<IServiceProvider, object> implementationFactory) where TService : class;

        IServiceManager AddScopedService<TService, TImplementation>(Func<IServiceProvider, TImplementation> implementationFactory) where TService : class where TImplementation : class, TService;

        IServiceManager AddSingletonService<TService>() where TService : class;

        IServiceManager AddSingletonService(Type serviceType, Type implementationType);

        IServiceManager AddSingletonService(Type serviceType, Func<IServiceProvider, object> implementationFactory);

        IServiceManager AddSingletonService<TService, TImplementation>() where TService : class where TImplementation : class, TService;

        IServiceManager AddSingletonService(Type serviceType);

        IServiceManager AddSingletonService<TService>(Func<IServiceProvider, object> implementationFactory) where TService : class;

        IServiceManager AddSingletonService<TService, TImplementation>(Func<IServiceProvider, TImplementation> implementationFactory) where TService : class where TImplementation : class, TService;

        IServiceManager AddSingletonService<TService>(TService implementationInstance) where TService : class;

        IServiceManager AddSingletonService(Type serviceType, object implementationInstance);

        IServiceManager AddTransientService<TService>() where TService : class;

        IServiceManager AddTransientService(Type serviceType, Type implementationType);

        IServiceManager AddTransientService(Type serviceType, Func<IServiceProvider, object> implementationFactory);

        IServiceManager AddTransientService<TService, TImplementation>() where TService : class where TImplementation : class, TService;

        IServiceManager AddTransientService(Type serviceType);

        IServiceManager AddTransientService<TService>(Func<IServiceProvider, object> implementationFactory) where TService : class;

        IServiceManager AddTransientService<TService, TImplementation>(Func<IServiceProvider, TImplementation> implementationFactory) where TService : class where TImplementation : class, TService;
    }
}
