using Qooba.Framework.Abstractions;
using Qooba.Framework.LifetimeManagers;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Qooba.Framework
{
    public class Container : BaseContainer
    {
        private static IDictionary<Type, IDictionary<object, Func<Type, object>>> container = new ConcurrentDictionary<Type, IDictionary<object, Func<Type, object>>>();

        private static IDictionary<Type, Func<IEnumerable<object>, object>> castingExpressions = new ConcurrentDictionary<Type, Func<IEnumerable<object>, object>>();

        public static IEnumerable<T> CastList<T>(IEnumerable<object> o) => o.Cast<T>().ToList();

        public override bool IsRegistered(Type typeToCheck, object keyToCheck)
        {
            var key = keyToCheck ?? string.Empty;
            return container.TryGetValue(typeToCheck, out IDictionary<object, Func<Type, object>> value) && value.ContainsKey(key);
        }

        public override IContainer RegisterInstance(object key, Type from, object instance)
        {
            InitializeContainer(from);
            object fromKey = PrepareKey(key);
            container[from][fromKey] = (x) => instance;
            return this;
        }

        public override IContainer RegisterType(object key, Type from, Type to, Lifetime lifetime)
        {
            InitializeContainer(from);
            object fromKey = PrepareKey(key);
            var ctor = to.GetConstructors().First();
            if (!from.GetTypeInfo().IsGenericTypeDefinition)
            {
                var activator = ObjectActivator.GetActivator(to, container, ctor);
                container[from][fromKey] = WrappWithLifetimeManager(lifetime, from, fromKey, (Func<Type, object>)activator);
            }
            else
            {
                Func<Type, object> f = (t) =>
                {
                    IDictionary<object, Func<Type, object>> activator;
                    Func<Type, object> act;
                    if (!(container.TryGetValue(t, out activator) && activator.TryGetValue(fromKey, out act)))
                    {
                        var p = t.GetGenericArguments();
                        var ft = to.MakeGenericType(p);
                        ctor = ft.GetConstructors().First();
                        act = (Func<Type, object>)ObjectActivator.GetActivator(ft, container, ctor);
                        if (!container.ContainsKey(t))
                        {
                            container[t] = new ConcurrentDictionary<object, Func<Type, object>>();
                        }

                        container[t][fromKey] = WrappWithLifetimeManager(lifetime, t, fromKey, act);
                    }

                    return act(t);
                };

                container[from][fromKey] = f;
            }

            return this;
        }

        public override IContainer RegisterFactory(object key, Type from, Func<IContainer, object> implementationFactory, Lifetime lifetime)
        {
            InitializeContainer(from);
            object fromKey = PrepareKey(key);
            container[from][fromKey] = WrappWithLifetimeManager(lifetime, from, fromKey, (t) => implementationFactory(this));
            return this;
        }

        public override object Resolve(object key, Type from)
        {
            object fromKey = PrepareKey(key);

            if (container.TryGetValue(from, out IDictionary<object, Func<Type, object>> df) && df.TryGetValue(fromKey, out Func<Type, object> f))
            {
                return f(from);
            }

            if (from.GetTypeInfo().IsGenericType)
            {
                var gt = from.GetGenericTypeDefinition();
                if (container.TryGetValue(gt, out df) && df.TryGetValue(fromKey, out f))
                {
                    InitializeContainer(from);
                    return f(from);
                }
            }

            if (!from.GetTypeInfo().IsAbstract && !from.GetTypeInfo().IsInterface)
            {
                this.RegisterType(fromKey, from, from, Lifetime.Transistent);
                return Resolve(fromKey, from);
            }

            if (from.GetInterfaces().Contains(typeof(IEnumerable)))
            {
                var arg = from.GetGenericArguments().FirstOrDefault();
                if (container.TryGetValue(arg, out df))
                {
                    var listTypeIn = typeof(List<>).MakeGenericType(new[] { typeof(object) });
                    var listTypeOut = typeof(List<>).MakeGenericType(new[] { arg });

                    Func<IEnumerable<object>, object> castingExpression;
                    if (!castingExpressions.TryGetValue(arg, out castingExpression))
                    {
                        var meth = typeof(Container).GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy).FirstOrDefault(x => x.Name == "CastList").MakeGenericMethod(arg);
                        var p = Expression.Parameter(typeof(IEnumerable<object>));
                        castingExpression = Expression.Lambda<Func<IEnumerable<object>, object>>(Expression.Call(null, meth, p), p).Compile();
                        castingExpressions[arg] = castingExpression;
                    }
                    var i = df.ToList().Select(x => x.Value(arg)).ToList();
                    return castingExpression(i);
                }
            }

            return null;
        }

        public override IEnumerable<object> ResolveAll(Type from)
        {
            var keys = container[from].Keys;
            foreach (var key in keys)
            {
                yield return this.Resolve(key, from);
            }
        }

        private static void InitializeContainer(Type type)
        {
            if (!container.ContainsKey(type))
            {
                container[type] = new ConcurrentDictionary<object, Func<Type, object>>();
            }
        }

        private Func<Type, object> WrappWithLifetimeManager(Lifetime lifetime, Type type, object fromKey, Func<Type, object> activator)
        {
            switch (lifetime)
            {
                case Lifetime.Transistent:
                    return (new TransistentLifetimeManager()).Resolve(type, fromKey, activator);
                case Lifetime.Singleton:
                    return (new SingletonLifetimeManager()).Resolve(type, fromKey, activator);
                default:
                    return (new TransistentLifetimeManager()).Resolve(type, fromKey, activator);
            }
        }

        private static object PrepareKey(object key) => key ?? string.Empty;
    }
}
