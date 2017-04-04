using System;

namespace Qooba.Framework.Abstractions
{
    public interface IServiceProvider : System.IServiceProvider
    {
        TService GetService<TService>() where TService : class;

        TService GetService<TService>(object key) where TService : class;

        object GetService(object key, Type serviceType);
    }
}
