using System.Collections.Generic;

namespace Qooba.Framework.Abstractions
{
    public interface IModuleBootstrapper
    {
        IList<IModule> GetModules();

        IModuleManager AddModule(IModule module);
    }
}
