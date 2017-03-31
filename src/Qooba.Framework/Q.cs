using Qooba.Framework.Abstractions;
using System;

namespace Qooba.Framework
{
    public class Q : IFramework
    {
        private readonly IServiceManager serviceManager;

        private readonly IModuleManager moduleManager;

        public Q(IModuleManager moduleManager, IServiceManager serviceManager)
        {
            this.moduleManager = moduleManager;
            this.serviceManager = serviceManager;
        }

        public IModuleManager AddModule(IModule module)
        {
            this.moduleManager.AddModule(module);
            return this;
        }

        public IModule GetModule(string name) => this.moduleManager.GetModule(name);

        public TService GetService<TService>() where TService : class => this.serviceManager.GetService<TService>();

        public object GetService(Type serviceType) => this.serviceManager.GetService(serviceType);

        public IServiceManager AddService(Func<IServiceDescriptor, IServiceDescriptor> serviceDescriptorFactory) => this.serviceManager.AddService(serviceDescriptorFactory);

        IServiceManager IServiceManagerEx.AddScopedService<TService>() => this.AddService(s => s.Service<TService>().Lifetime(Lifetime.Scoped));

        IServiceManager IServiceManagerEx.AddScopedService(Type serviceType, Type implementationType) => this.AddService(s => s.Service(serviceType).As(implementationType).Lifetime(Lifetime.Scoped));

        IServiceManager IServiceManagerEx.AddScopedService(Type serviceType, Func<Abstractions.IServiceProvider, object> implementationFactory) => this.AddService(s => s.Service(serviceType).As(implementationFactory).Lifetime(Lifetime.Scoped));

        IServiceManager IServiceManagerEx.AddScopedService(Type serviceType) => this.AddService(s => s.Service(serviceType).Lifetime(Lifetime.Scoped));

        IServiceManager IServiceManagerEx.AddScopedService<TService>(Func<Abstractions.IServiceProvider, object> implementationFactory) => this.AddService(s => s.Service<TService>().As(implementationFactory).Lifetime(Lifetime.Scoped));

        IServiceManager IServiceManagerEx.AddScopedService<TService, TImplementation>() => this.AddService(s => s.Service<TService>().As<TImplementation>().Lifetime(Lifetime.Scoped));

        IServiceManager IServiceManagerEx.AddScopedService<TService, TImplementation>(Func<Abstractions.IServiceProvider, TImplementation> implementationFactory) => this.AddService(s => s.Service<TService>().As(implementationFactory).Lifetime(Lifetime.Scoped));

        IServiceManager IServiceManagerEx.AddSingletonService<TService, TImplementation>() => this.AddService(s => s.Service<TService>().As<TImplementation>().Lifetime(Lifetime.Singleton));

        IServiceManager IServiceManagerEx.AddSingletonService<TService, TImplementation>(Func<Abstractions.IServiceProvider, TImplementation> implementationFactory) => this.AddService(s => s.Service<TService>().As(implementationFactory).Lifetime(Lifetime.Singleton));

        IServiceManager IServiceManagerEx.AddSingletonService<TService>() => this.AddService(s => s.Service<TService>().Lifetime(Lifetime.Singleton));

        IServiceManager IServiceManagerEx.AddSingletonService(Type serviceType, Type implementationType) => this.AddService(s => s.Service(serviceType).As(implementationType).Lifetime(Lifetime.Singleton));

        IServiceManager IServiceManagerEx.AddSingletonService(Type serviceType, Func<Abstractions.IServiceProvider, object> implementationFactory) => this.AddService(s => s.Service(serviceType).As(implementationFactory).Lifetime(Lifetime.Singleton));

        IServiceManager IServiceManagerEx.AddSingletonService(Type serviceType) => this.AddService(s => s.Service(serviceType).Lifetime(Lifetime.Singleton));

        IServiceManager IServiceManagerEx.AddSingletonService<TService>(Func<Abstractions.IServiceProvider, object> implementationFactory) => this.AddService(s => s.Service<TService>().As(implementationFactory).Lifetime(Lifetime.Singleton));

        IServiceManager IServiceManagerEx.AddSingletonService<TService>(TService implementationInstance) => this.AddService(s => s.Service<TService>().As(implementationInstance).Lifetime(Lifetime.Singleton));

        IServiceManager IServiceManagerEx.AddSingletonService(Type serviceType, object implementationInstance) => this.AddService(s => s.Service(serviceType).As(implementationInstance).Lifetime(Lifetime.Singleton));

        IServiceManager IServiceManagerEx.AddTransientService<TService>() => this.AddService(s => s.Service<TService>().Lifetime(Lifetime.Transistent));

        IServiceManager IServiceManagerEx.AddTransientService(Type serviceType, Type implementationType) => this.AddService(s => s.Service(serviceType).As(implementationType).Lifetime(Lifetime.Transistent));

        IServiceManager IServiceManagerEx.AddTransientService(Type serviceType, Func<Abstractions.IServiceProvider, object> implementationFactory) => this.AddService(s => s.Service(serviceType).As(implementationFactory).Lifetime(Lifetime.Transistent));

        IServiceManager IServiceManagerEx.AddTransientService(Type serviceType) => this.AddService(s => s.Service(serviceType).Lifetime(Lifetime.Transistent));

        IServiceManager IServiceManagerEx.AddTransientService<TService>(Func<Abstractions.IServiceProvider, object> implementationFactory) => this.AddService(s => s.Service<TService>().As(implementationFactory).Lifetime(Lifetime.Transistent));

        IServiceManager IServiceManagerEx.AddTransientService<TService, TImplementation>() => this.AddService(s => s.Service<TService>().As<TImplementation>().Lifetime(Lifetime.Transistent));

        IServiceManager IServiceManagerEx.AddTransientService<TService, TImplementation>(Func<Abstractions.IServiceProvider, TImplementation> implementationFactory) => this.AddService(s => s.Service<TService>().As(implementationFactory).Lifetime(Lifetime.Transistent));
    }
}
