using System;

namespace Qooba.Framework.Abstractions
{
    public interface IServiceManager : IServiceProvider
    {
        IServiceManager AddService(Func<IServiceDescriptor, IServiceDescriptor> serviceDescriptorFactory);
    }
}
