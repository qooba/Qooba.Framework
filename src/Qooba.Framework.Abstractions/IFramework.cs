using System;

namespace Qooba.Framework.Abstractions
{
    public interface IFramework
    {
        IFramework AddAssembly(Func<IAssemblyDescriptor, IAssemblyDescriptor> assemblyDescriptorFactory);

        IFramework AddModule(Func<IModuleDescriptor, IModuleDescriptor> moduleDescriptorFactory);

        IFramework AddService(Func<IServiceDescriptor, IServiceDescriptor> serviceDescriptorFactory);

        IServiceProvider Bootstrapp();

        IServiceProvider BootstrappAuto();

        IFramework AddScopedService<TService>() where TService : class;

        IFramework AddScopedService(Type serviceType, Type implementationType);

        IFramework AddScopedService(Type serviceType, Func<IServiceProvider, object> implementationFactory);

        IFramework AddScopedService<TService, TImplementation>() where TService : class where TImplementation : class, TService;

        IFramework AddScopedService(Type serviceType);

        IFramework AddScopedService<TService>(Func<IServiceProvider, object> implementationFactory) where TService : class;

        IFramework AddScopedService<TService, TImplementation>(Func<IServiceProvider, TImplementation> implementationFactory) where TService : class where TImplementation : class, TService;

        IFramework AddSingletonService<TService>() where TService : class;

        IFramework AddSingletonService(Type serviceType, Type implementationType);

        IFramework AddSingletonService(Type serviceType, Func<IServiceProvider, object> implementationFactory);

        IFramework AddSingletonServiceWithConfiguration(Type serviceType, Func<IConfiguration, object> implementationInstanceFunc);

        IFramework AddSingletonService<TService, TImplementation>() where TService : class where TImplementation : class, TService;

        IFramework AddSingletonService(Type serviceType);

        IFramework AddSingletonService<TService>(Func<IServiceProvider, object> implementationFactory) where TService : class;

        IFramework AddSingletonService<TService, TImplementation>(Func<IServiceProvider, TImplementation> implementationFactory) where TService : class where TImplementation : class, TService;

        IFramework AddSingletonService<TService>(TService implementationInstance) where TService : class;

        IFramework AddSingletonService(Type serviceType, object implementationInstance);

        IFramework AddTransientService<TService>() where TService : class;

        IFramework AddTransientService(Type serviceType, Type implementationType);

        IFramework AddTransientService(Type serviceType, Func<IServiceProvider, object> implementationFactory);

        IFramework AddTransientService<TService, TImplementation>() where TService : class where TImplementation : class, TService;

        IFramework AddTransientService(Type serviceType);

        IFramework AddTransientService<TService>(Func<IServiceProvider, object> implementationFactory) where TService : class;

        IFramework AddTransientService<TService, TImplementation>(Func<IServiceProvider, TImplementation> implementationFactory) where TService : class where TImplementation : class, TService;
    }
}