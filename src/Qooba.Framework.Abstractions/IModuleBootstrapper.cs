using System.Collections.Generic;

namespace Qooba.Framework.Abstractions
{
    public interface IModuleBootstrapper
    {
        void BootstrappModules(params string[] includeModuleNamePattern);

        IList<IModule> GetModules();
    }
}
