using System.Collections.Concurrent;

namespace Qooba.Framework
{
    public class Bootstrapper
    {
        private static ConcurrentBag<bool> bootstrapped = new ConcurrentBag<bool>();

        public static void Bootstrapp(params string[] includeModuleNamePattern)
        {
            if (bootstrapped.IsEmpty)
            {
                bootstrapped.Add(true);
                PreApplicationInit.InitializeModules(includeModuleNamePattern);
                ModuleManager.Current.Bootstrapp();
            }
        }
    }
}