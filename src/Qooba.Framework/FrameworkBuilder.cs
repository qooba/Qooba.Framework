using Qooba.Framework.Abstractions;

namespace Qooba.Framework
{
    public static class FrameworkBuilder
    {
        public static IFramework Create()
        {
            var assemblyManager = new AssemblyManager();
            var moduleManager = new ModuleManager();
            var serviceManager = new ServiceManager();
            var bootstrapper = new Bootstrapper(moduleManager, serviceManager, assemblyManager);
            var q = new Framework(assemblyManager, moduleManager, serviceManager, bootstrapper);
            bootstrapper.SetFramework(q);
            return q;
        }
    }
}
