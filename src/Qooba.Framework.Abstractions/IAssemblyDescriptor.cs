using System.Reflection;

namespace Qooba.Framework.Abstractions
{
    public interface IAssemblyDescriptor
    {
        IAssemblyDescriptor Assembly(Assembly assembly);

        IAssemblyDescriptor Path(string assemblyPath);

        IAssemblyDescriptor Pattern(string assemblyPattern);
    }
}
