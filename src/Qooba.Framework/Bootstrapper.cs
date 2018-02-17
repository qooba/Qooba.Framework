using Qooba.Framework.Abstractions;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;

namespace Qooba.Framework
{
    public class Bootstrapper : IBootstrapper
    {
        private readonly IModuleBootstrapper moduleBootstrapper;

        private readonly IServiceBootstrapper serviceBootstrapper;

        private readonly IAssemblyBootstrapper assemblyBootstrapper;

        private IFramework framework;

        private static ConcurrentBag<bool> bootstrapped = new ConcurrentBag<bool>();

        public Bootstrapper(IModuleBootstrapper moduleBootstrapper, IServiceBootstrapper serviceBootstrapper, IAssemblyBootstrapper assemblyBootstrapper)
        {
            this.moduleBootstrapper = moduleBootstrapper;
            this.serviceBootstrapper = serviceBootstrapper;
            this.assemblyBootstrapper = assemblyBootstrapper;
        }

        internal void SetFramework(IFramework framework)
        {
            this.framework = framework;
        }

        public void Bootstrapp()
        {
            this.PrepareModules();
            IServiceManager serviceManager = null;
            var modules = this.moduleBootstrapper.GetModules();

            var containerBootstrapper = modules.FirstOrDefault(x => x is IServiceManagerModule);
            if (containerBootstrapper != null)
            {
                serviceManager = ((IServiceManagerModule)containerBootstrapper).CreateServiceManager();
                this.serviceBootstrapper.SetServiceManager(serviceManager);
            }
            else
            {
                var container = new Container();
                container.RegisterInstance(null, typeof(Abstractions.IServiceProvider), container);
                container.RegisterInstance(null, typeof(IContainer), container);
                this.serviceBootstrapper.SetServiceManager(container);
            }

            foreach (var module in modules)
            {
                module.Bootstrapp(this.framework);
            }

            foreach (var service in this.serviceBootstrapper.GetServices())
            {
                serviceManager.AddService(service);
            }
        }

        private void BootstrappCore(IServiceManager serviceManager)
        {
            serviceManager.AddService(s => s.Service<IConfiguration>().As(new Configuration()).Lifetime(Lifetime.Singleton));
            serviceManager.AddService(s => s.Service<IFactory>().As<Factory>().Lifetime(Lifetime.Singleton));
            serviceManager.AddService(s => s.Service(typeof(IFactory<>)).As(typeof(Factory<>)).Lifetime(Lifetime.Singleton));
            serviceManager.AddService(s => s.Service<ILogger>().As<Logger>().Lifetime(Lifetime.Singleton));
        }

        private void PrepareModules()
        {
            var assemblies = this.assemblyBootstrapper.GetAssemblies();

            foreach (var assembly in assemblies)
            {
                var type = assembly.GetTypes().Where(t => t.GetInterfaces().Contains(typeof(IModule))).FirstOrDefault();
                var typeInfo = type?.GetTypeInfo();
                if (type != null && typeInfo != null && !typeInfo.IsAbstract && !typeInfo.IsInterface)
                {
                    var module = (IModule)Activator.CreateInstance(type);
                    this.moduleBootstrapper.AddModule(module);
                }
            }
        }
    }
}