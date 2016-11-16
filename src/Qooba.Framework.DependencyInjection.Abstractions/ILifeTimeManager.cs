using System;

namespace Qooba.Framework.DependencyInjection.Abstractions
{
    public interface ILifetimeManager
    {
        Lifetime Lifetime { get; }

        Func<Type, object> Resolve(Type type, Func<Type, object> activator);
    }
}
