using Qooba.Framework.Abstractions;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Qooba.Framework.LifetimeManagers
{
    public class SingletonLifetimeManager : ILifetimeManager
    {
        private readonly static IDictionary<Type, ConcurrentDictionary<object, Lazy<object>>> singletons = new ConcurrentDictionary<Type, ConcurrentDictionary<object, Lazy<object>>>();

        public Lifetime Lifetime
        {
            get
            {
                return Lifetime.Singleton;
            }
        }

        public Func<Type, object> Resolve(Type type, object fromKey, Func<Type, object> activator)
        {
            if (!singletons.TryGetValue(type, out ConcurrentDictionary<object, Lazy<object>> dict))
            {
                singletons[type] = dict = new ConcurrentDictionary<object, Lazy<object>>();
            }

            if (!dict.TryGetValue(fromKey, out Lazy<object> instance))
            {
                dict[fromKey] = instance = new Lazy<object>(() => activator(type));
            }

            return (t) => instance.Value;
        }
    }
}
