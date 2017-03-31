using System.Collections.Generic;
using System.Reflection;

namespace Qooba.Framework.Abstractions
{
    public interface IAssemblyBootstrapper
    {
        IList<Assembly> GetAssemblies();
    }
}
