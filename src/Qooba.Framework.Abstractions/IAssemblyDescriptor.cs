using System.Reflection;

namespace Qooba.Framework.Abstractions
{
    public interface IAssemblyDescriptor
    {
        IAssemblyDescriptor All();

        IAssemblyDescriptor All(string assembliesPath);

        IAssemblyDescriptor Assembly(Assembly assembly);

        IAssemblyDescriptor AssemblyPath(string assemblyPath);

        IAssemblyDescriptor Pattern(string assemblyPattern);
    }
}
