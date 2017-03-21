using Qooba.Framework.Abstractions;
using System.Collections.Concurrent;

namespace Qooba.Framework
{
    public class Bootstrapper : IBootstrapper
    {
        private static ConcurrentBag<bool> bootstrapped = new ConcurrentBag<bool>();

        public static IBootstrapper Instance => new Bootstrapper();

        public IContainer Container => ContainerManager.Container;

        public IBootstrapper Bootstrapp(params string[] includeModuleNamePattern)
        {
            if (bootstrapped.IsEmpty)
            {
                bootstrapped.Add(true);
                PreApplicationInit.InitializeModules(includeModuleNamePattern);
                ModuleManager.Current.Bootstrapp();
            }

            return this;
        }

        public IBootstrapper BootstrappModules(params IModule[] includeModules)
        {
            if (bootstrapped.IsEmpty)
            {
                bootstrapped.Add(true);
                for (int i = 0; i < includeModules.Length; i++)
                {
                    ModuleManager.Current.Modules.Add(includeModules[i], null);
                }

                ModuleManager.Current.Bootstrapp();
            }

            return this;
        }
    }
}