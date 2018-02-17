using Qooba.Framework.Abstractions;
using System;

namespace Qooba.Framework.LifetimeManagers
{
    public class TransistentLifetimeManager : ILifetimeManager
    {
        public Lifetime Lifetime => Lifetime.Transistent;

        public Func<Type, object> Resolve(Type type, object fromKey, Func<Type, object> activator) => activator;
    }
}
