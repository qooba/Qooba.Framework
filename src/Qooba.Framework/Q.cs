using Qooba.Framework.Abstractions;
using System;
using System.Collections.Generic;

namespace Qooba.Framework
{
    public class Q : IFramework, IFrameworkManager
    {
        private readonly IServiceManager serviceManager;

        private readonly IModuleManager moduleManager;

        private readonly IAssemblyManager assemblyManager;

        private readonly IBootstrapper bootstrapper;

        public Q(IAssemblyManager assemblyManager, IModuleManager moduleManager, IServiceManager serviceManager, IBootstrapper bootstrapper)
        {
            this.assemblyManager = assemblyManager;
            this.moduleManager = moduleManager;
            this.serviceManager = serviceManager;
            this.bootstrapper = bootstrapper;
        }

        public static IFramework Create()
        {
            var assemblyManager = new AssemblyManager();
            var moduleManager = new ModuleManager();
            var serviceManager = new ServiceManager();
            var bootstrapper = new Bootstrapper(moduleManager, serviceManager, assemblyManager);
            return new Q(assemblyManager, moduleManager, serviceManager, bootstrapper);
        }

        public IFramework AddAssembly(Func<IAssemblyDescriptor, IAssemblyDescriptor> assemblyDescriptorFactory)
        {
            this.assemblyManager.AddAssembly(assemblyDescriptorFactory);
            return this;
        }

        public IFramework AddModule(Func<IModuleDescriptor, IModuleDescriptor> moduleDescriptorFactory)
        {
            this.moduleManager.AddModule(moduleDescriptorFactory);
            return this;
        }

        public IFramework AddService(Func<IServiceDescriptor, IServiceDescriptor> serviceDescriptorFactory)
        {
            this.serviceManager.AddService(serviceDescriptorFactory);
            return this;
        }

        IFrameworkManager IFrameworkManager.AddAssembly(Func<IAssemblyDescriptor, IAssemblyDescriptor> assemblyDescriptorFactory)
        {
            this.assemblyManager.AddAssembly(assemblyDescriptorFactory);
            return this;
        }

        IFrameworkManager IFrameworkManager.AddModule(Func<IModuleDescriptor, IModuleDescriptor> moduleDescriptorFactory)
        {
            this.moduleManager.AddModule(moduleDescriptorFactory);
            return this;
        }

        IFrameworkManager IFrameworkManager.AddService(Func<IServiceDescriptor, IServiceDescriptor> serviceDescriptorFactory)
        {
            this.serviceManager.AddService(serviceDescriptorFactory);
            return this;
        }

        public IModule GetModule(string name) => this.moduleManager.GetModule(name);

        public TService GetService<TService>() where TService : class => this.serviceManager.GetService<TService>();

        public object GetService(Type serviceType) => this.serviceManager.GetService(serviceType);

        public TService GetService<TService>(object key) where TService : class => this.serviceManager.GetService<TService>(key);

        public object GetService(object key, Type serviceType) => this.serviceManager.GetService(key, serviceType);

        public IEnumerable<TService> GetServices<TService>() where TService : class => this.serviceManager.GetServices<TService>();

        public IEnumerable<object> GetServices(Type serviceType) => this.serviceManager.GetServices(serviceType);

        public Abstractions.IServiceProvider Bootstrapp()
        {
            this.bootstrapper.Bootstrapp();
            return this;
        }

        public Abstractions.IServiceProvider BootstrappAuto()
        {
            this.assemblyManager.AddAssembly(a => a.All());
            return this.Bootstrapp();
        }

        public IFramework AddScopedService<TService>() where TService : class => this.AddService(s => s.Service<TService>().Lifetime(Lifetime.Scoped));

        public IFramework AddScopedService(Type serviceType, Type implementationType) => this.AddService(s => s.Service(serviceType).As(implementationType).Lifetime(Lifetime.Scoped));

        public IFramework AddScopedService(Type serviceType, Func<Abstractions.IServiceProvider, object> implementationFactory) => this.AddService(s => s.Service(serviceType).As(implementationFactory).Lifetime(Lifetime.Scoped));

        public IFramework AddScopedService(Type serviceType) => this.AddService(s => s.Service(serviceType).Lifetime(Lifetime.Scoped));

        public IFramework AddScopedService<TService>(Func<Abstractions.IServiceProvider, object> implementationFactory)
            where TService : class
            => this.AddService(s => s.Service<TService>().As(implementationFactory).Lifetime(Lifetime.Scoped));

        public IFramework AddScopedService<TService, TImplementation>()
            where TService : class
            where TImplementation : class, TService
            => this.AddService(s => s.Service<TService>().As<TImplementation>().Lifetime(Lifetime.Scoped));

        public IFramework AddScopedService<TService, TImplementation>(Func<Abstractions.IServiceProvider, TImplementation> implementationFactory)
            where TService : class
            where TImplementation : class, TService
            => this.AddService(s => s.Service<TService>().As(implementationFactory).Lifetime(Lifetime.Scoped));

        public IFramework AddSingletonService<TService, TImplementation>()
            where TService : class
            where TImplementation : class, TService
            => this.AddService(s => s.Service<TService>().As<TImplementation>().Lifetime(Lifetime.Singleton));

        public IFramework AddSingletonService<TService, TImplementation>(Func<Abstractions.IServiceProvider, TImplementation> implementationFactory)
            where TService : class
            where TImplementation : class, TService
            => this.AddService(s => s.Service<TService>().As(implementationFactory).Lifetime(Lifetime.Singleton));

        public IFramework AddSingletonService<TService>()
            where TService : class
            => this.AddService(s => s.Service<TService>().Lifetime(Lifetime.Singleton));

        public IFramework AddSingletonService(Type serviceType, Type implementationType) => this.AddService(s => s.Service(serviceType).As(implementationType).Lifetime(Lifetime.Singleton));

        public IFramework AddSingletonService(Type serviceType, Func<Abstractions.IServiceProvider, object> implementationFactory) => this.AddService(s => s.Service(serviceType).As(implementationFactory).Lifetime(Lifetime.Singleton));

        public IFramework AddSingletonService(Type serviceType) => this.AddService(s => s.Service(serviceType).Lifetime(Lifetime.Singleton));

        public IFramework AddSingletonService<TService>(Func<Abstractions.IServiceProvider, object> implementationFactory)
            where TService : class
            => this.AddService(s => s.Service<TService>().As(implementationFactory).Lifetime(Lifetime.Singleton));

        public IFramework AddSingletonService<TService>(TService implementationInstance)
            where TService : class
            => this.AddService(s => s.Service<TService>().As(implementationInstance).Lifetime(Lifetime.Singleton));

        public IFramework AddSingletonService(Type serviceType, object implementationInstance) => this.AddService(s => s.Service(serviceType).As(implementationInstance).Lifetime(Lifetime.Singleton));

        public IFramework AddTransientService<TService>()
            where TService : class
            => this.AddService(s => s.Service<TService>().Lifetime(Lifetime.Transistent));

        public IFramework AddTransientService(Type serviceType, Type implementationType) => this.AddService(s => s.Service(serviceType).As(implementationType).Lifetime(Lifetime.Transistent));

        public IFramework AddTransientService(Type serviceType, Func<Abstractions.IServiceProvider, object> implementationFactory) => this.AddService(s => s.Service(serviceType).As(implementationFactory).Lifetime(Lifetime.Transistent));

        public IFramework AddTransientService(Type serviceType) => this.AddService(s => s.Service(serviceType).Lifetime(Lifetime.Transistent));

        public IFramework AddTransientService<TService>(Func<Abstractions.IServiceProvider, object> implementationFactory)
            where TService : class
            => this.AddService(s => s.Service<TService>().As(implementationFactory).Lifetime(Lifetime.Transistent));

        public IFramework AddTransientService<TService, TImplementation>()
            where TService : class
            where TImplementation : class, TService
            => this.AddService(s => s.Service<TService>().As<TImplementation>().Lifetime(Lifetime.Transistent));

        public IFramework AddTransientService<TService, TImplementation>(Func<Abstractions.IServiceProvider, TImplementation> implementationFactory)
            where TService : class
            where TImplementation : class, TService
            => this.AddService(s => s.Service<TService>().As(implementationFactory).Lifetime(Lifetime.Transistent));
    }
}
