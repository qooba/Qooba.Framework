using System;
using System.Collections.Generic;

namespace Qooba.Framework.Abstractions
{
    public interface IServiceBootstrapper
    {
        void SetServiceManager(IServiceManager serviceManager);

        IEnumerable<Func<IServiceDescriptor, IServiceDescriptor>> GetServices();
    }
}
