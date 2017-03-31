using System;

namespace Qooba.Framework.Abstractions
{
    public interface IFramework
    {
        IFramework AddAssembly(Func<IAssemblyDescriptor, IAssemblyDescriptor> assemblyDescriptorFactory);

        IFramework AddModule(Func<IModuleDescriptor, IModuleDescriptor> moduleDescriptorFactory);

        IFramework AddService(Func<IServiceDescriptor, IServiceDescriptor> serviceDescriptorFactory);

        IServiceProvider Bootstrapp();
    }
}