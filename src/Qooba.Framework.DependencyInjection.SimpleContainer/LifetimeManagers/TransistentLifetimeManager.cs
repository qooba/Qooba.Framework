using Qooba.Framework.Abstractions;
using Qooba.Framework.DependencyInjection.Abstractions;
using System;

namespace Qooba.Framework.DependencyInjection.SimpleContainer.LifetimeManagers
{
    public class TransistentLifetimeManager : ILifetimeManager
    {
        public Lifetime Lifetime => Lifetime.Transistent;

        public Func<Type, object> Resolve(Type type, object fromKey, Func<Type, object> activator) => activator;
    }
}
