using System;

namespace Qooba.Framework.Abstractions
{
    public interface IAssemblyManager
    {
        IAssemblyManager AddAssembly(Func<IAssemblyDescriptor, IAssemblyDescriptor> assemblyDescriptorFactory);
    }
}
