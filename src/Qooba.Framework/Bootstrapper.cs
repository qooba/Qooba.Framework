using Qooba.Framework.Abstractions;
using System.Collections.Concurrent;
using System.Linq;

namespace Qooba.Framework
{
    public class Bootstrapper : IBootstrapper
    {
        private readonly IModuleBootstrapper moduleBootstrapper;

        private readonly IServiceBootstrapper serviceBootstrapper;

        private readonly IFramework framework;

        private static ConcurrentBag<bool> bootstrapped = new ConcurrentBag<bool>();

        public static IBootstrapper Instance => new Bootstrapper(ModuleManager.Current, ServiceManager.Current, new Q(ModuleManager.Current, ServiceManager.Current));

        public Bootstrapper(IModuleBootstrapper moduleBootstrapper, IServiceBootstrapper serviceBootstrapper, IFramework framework)
        {
            this.moduleBootstrapper = moduleBootstrapper;
            this.serviceBootstrapper = serviceBootstrapper;
            this.framework = framework;
        }

        public IFramework Bootstrapp(params string[] includeModuleNamePattern)
        {
            if (bootstrapped.IsEmpty)
            {
                bootstrapped.Add(true);
                this.moduleBootstrapper.BootstrappModules(includeModuleNamePattern);
                this.Bootstrapp();
            }

            return this.framework;
        }

        public IFramework BootstrappModules(params IModule[] includeModules)
        {
            if (bootstrapped.IsEmpty)
            {
                bootstrapped.Add(true);
                for (int i = 0; i < includeModules.Length; i++)
                {
                    ModuleManager.Current.Modules.Add(includeModules[i], null);
                }

                this.Bootstrapp();
            }

            return this.framework;
        }

        private void Bootstrapp()
        {
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
    }
}