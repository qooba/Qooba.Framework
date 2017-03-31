using System;

namespace Qooba.Framework.Abstractions
{
    public interface IServiceProvider
    {
        object GetService(Type serviceType);

        TService GetService<TService>() where TService : class;
    }
}
