using Qooba.Framework.Abstractions;
using System;

namespace Qooba.Framework.DependencyInjection.Abstractions
{
    public interface ILifetimeManager
    {
        Lifetime Lifetime { get; }

        Func<Type, object> Resolve(Type type, object fromKey, Func<Type, object> activator);
    }
}
