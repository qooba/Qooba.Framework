using Qooba.Framework.Abstractions;
using System;

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

        IFramework IFramework.AddAssembly(Func<IAssemblyDescriptor, IAssemblyDescriptor> assemblyDescriptorFactory)
        {
            this.assemblyManager.AddAssembly(assemblyDescriptorFactory);
            return this;
        }

        IFramework IFramework.AddModule(Func<IModuleDescriptor, IModuleDescriptor> moduleDescriptorFactory)
        {
            this.moduleManager.AddModule(moduleDescriptorFactory);
            return this;
        }

        IFramework IFramework.AddService(Func<IServiceDescriptor, IServiceDescriptor> serviceDescriptorFactory)
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

        public Abstractions.IServiceProvider Bootstrapp()
        {
            this.bootstrapper.Bootstrapp();
            return this;
        }
    }
}
