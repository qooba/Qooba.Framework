using System;

namespace Qooba.Framework.Abstractions
{
    public interface IFrameworkManager : IServiceProvider
    {
        IFrameworkManager AddAssembly(Func<IAssemblyDescriptor, IAssemblyDescriptor> assemblyDescriptorFactory);

        IFrameworkManager AddModule(Func<IModuleDescriptor, IModuleDescriptor> moduleDescriptorFactory);

        IFrameworkManager AddService(Func<IServiceDescriptor, IServiceDescriptor> serviceDescriptorFactory);
    }
}