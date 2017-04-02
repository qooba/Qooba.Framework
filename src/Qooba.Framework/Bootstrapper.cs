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

        private readonly IFramework framework;

        private static ConcurrentBag<bool> bootstrapped = new ConcurrentBag<bool>();

        public Bootstrapper(IModuleBootstrapper moduleBootstrapper, IServiceBootstrapper serviceBootstrapper, IAssemblyBootstrapper assemblyBootstrapper)
        {
            this.moduleBootstrapper = moduleBootstrapper;
            this.serviceBootstrapper = serviceBootstrapper;
            this.assemblyBootstrapper = assemblyBootstrapper;
        }

        public void Bootstrapp()
        {
            this.PrepareModules();
            IServiceManager serviceManager;
            var modules = this.moduleBootstrapper.GetModules();
            
            var containerBootstrapper = modules.FirstOrDefault(x => x is IServiceManagerModule);
            if (containerBootstrapper != null)
            {
                serviceManager = ((IServiceManagerModule)containerBootstrapper).CreateServiceManager();
                this.serviceBootstrapper.SetServiceManager(serviceManager);
            }

            foreach (var module in modules)
            {
                module.Bootstrapp(this.framework);
            }
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