using Qooba.Framework.DependencyInjection.Abstractions;
using System;

namespace Qooba.Framework.DependencyInjection.SimpleContainer.LifetimeManagers
{
    public class TransistentLifetimeManager : ILifetimeManager
    {
        public Lifetime Lifetime
        {
            get
            {
                return Lifetime.Transistent;
            }
        }

        public Func<Type, object> Resolve(Type type, Func<Type, object> activator)
        {
            return activator;
        }
    }
}
