using System;

namespace Qooba.Framework.Abstractions
{
    public interface IModuleManager : IModuleProvider
    {
        IModuleManager AddModule(Func<IModuleDescriptor, IModuleDescriptor> moduleDescriptorFactory);
    }
}
