using Qooba.Framework.DependencyInjection.Abstractions;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Qooba.Framework.DependencyInjection.SimpleContainer.LifetimeManagers
{
    public class SingletonLifetimeManager : ILifetimeManager
    {
        private readonly static IDictionary<Type, Lazy<object>> singletons = new ConcurrentDictionary<Type, Lazy<object>>();

        public Lifetime Lifetime
        {
            get
            {
                return Lifetime.Singleton;
            }
        }

        public Func<Type, object> Resolve(Type type, Func<Type, object> activator)
        {
            Lazy<object> instance;
            if(!singletons.TryGetValue(type, out instance))
            {
                instance = new Lazy<object>(() => activator(type));
                singletons[type] = instance;
            }
            
            return (t) => instance.Value;
        }
    }
}
