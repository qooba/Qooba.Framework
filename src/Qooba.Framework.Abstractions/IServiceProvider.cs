using System;
using System.Collections.Generic;

namespace Qooba.Framework.Abstractions
{
    public interface IServiceProvider : System.IServiceProvider
    {
        TService GetService<TService>() where TService : class;

        TService GetService<TService>(object key) where TService : class;

        object GetService(object key, Type serviceType);

        IEnumerable<TService> GetServices<TService>() where TService : class;

        IEnumerable<object> GetServices(Type serviceType);
    }
}
