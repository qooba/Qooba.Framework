//using Microsoft.Extensions.DependencyInjection;
using Qooba.Framework.Abstractions;
using Qooba.Framework.DependencyInjection.SimpleContainer.LifetimeManagers;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Qooba.Framework.DependencyInjection.SimpleContainer
{
    public class Container : IContainer
    {
        private static IDictionary<Type, IDictionary<object, Func<Type, object>>> container = new ConcurrentDictionary<Type, IDictionary<object, Func<Type, object>>>();

        private static IDictionary<Type, Func<IEnumerable<object>, object>> castingExpressions = new ConcurrentDictionary<Type, Func<IEnumerable<object>, object>>();

        public bool IsRegistered<T>() where T : class => container.ContainsKey(typeof(T));

        public bool IsRegistered<T>(object keyToCheck) 
            where T : class
        {
            return container.TryGetValue(typeof(T), out IDictionary<object, Func<Type, object>> dictionary) && dictionary.ContainsKey(keyToCheck);
        }

        public bool IsRegistered(Type typeToCheck)
        {
            throw new NotImplementedException();
        }

        public bool IsRegistered(Type typeToCheck, object keyToCheck)
        {
            throw new NotImplementedException();
        }

        public IContainer RegisterInstance<TInterface>(TInterface instance)
        {
            var type = typeof(TInterface);
            InitializeContainer(type);

            container[type][""] = (t) => instance;
            return this;
        }

        public IContainer RegisterInstance<TInterface>(object key, TInterface instance)
        {
            throw new NotImplementedException();
        }

        public IContainer RegisterInstance<TInterface>(TInterface instance, Lifetime lifetime)
        {
            throw new NotImplementedException();
        }

        public IContainer RegisterInstance(Type t, object instance)
        {
            return this.RegisterInstance(t, instance, "");
        }

        public IContainer RegisterInstance(Type t, object instance, object key)
        {
            InitializeContainer(t);

            container[t][key] = (x) => instance;
            return this;
        }

        public IContainer RegisterInstance<TInterface>(object key, TInterface instance, Lifetime lifetime)
        {
            throw new NotImplementedException();
        }

        public IContainer RegisterInstance(Type t, object instance, Lifetime lifetime)
        {
            throw new NotImplementedException();
        }

        public IContainer RegisterType<T>()
            where T : class
        {
            throw new NotImplementedException();
        }

        public IContainer RegisterType<T>(Lifetime lifetime)
        {
            throw new NotImplementedException();
        }

        public IContainer RegisterType<TFrom, TTo>()
            where TTo : class, TFrom
            where TFrom : class
        {
            return this.RegisterType<TFrom, TTo>("");
        }

        public IContainer RegisterType<TFrom, TTo>(Lifetime lifetime)
            where TTo : class, TFrom
            where TFrom : class
        {
            return this.RegisterType(typeof(TFrom), typeof(TTo), lifetime);
        }

        public IContainer RegisterType<TFrom, TTo>(object key) where TTo : class, TFrom
        {
            var type = typeof(TFrom);
            InitializeContainer(type);

            return RegisterType(typeof(TFrom), typeof(TTo), key);
        }

        public IContainer RegisterType<T>(object key)
            where T : class
        {
            return this.RegisterType<T, T>(key);
        }

        public IContainer RegisterType(Type t)
        {
            return RegisterType(t, t);
        }

        public IContainer RegisterType<T>(object key, Lifetime lifetime)
            where T : class
        {
            var type = typeof(T);
            return RegisterType(type, type, key);
        }

        public IContainer RegisterType<TFrom, TTo>(object key, Lifetime lifetime) where TTo : TFrom
        {
            return RegisterType(typeof(TFrom), typeof(TTo), key, lifetime);
        }

        public IContainer RegisterType(Type t, Lifetime lifetime)
        {
            return RegisterType(t, t, lifetime);
        }

        public IContainer RegisterType(Type t, object key)
        {
            return RegisterType(t, t, key);
        }

        public IContainer RegisterType(Type from, Type to)
        {
            return RegisterType(from, to, "");
        }

        public IContainer RegisterType(Type t, object key, Lifetime lifetime)
        {
            return RegisterType(t, t, key, lifetime);
        }

        public IContainer RegisterType(Type from, Type to, Lifetime lifetime)
        {
            return this.RegisterType(from, to, string.Empty, lifetime);
        }

        public IContainer RegisterType(Type from, Type to, object key)
        {
            return this.RegisterType(from, to, key, Lifetime.Transistent);
        }

        public IContainer RegisterType(Type from, Type to, object key, Lifetime lifetime)
        {
            InitializeContainer(from);
            var ctor = to.GetConstructors().First();
            if (!from.GetTypeInfo().IsGenericTypeDefinition)
            {
                var activator = ObjectActivator.GetActivator(to, container, ctor);
                container[from][key] = WrappWithLifetimeManager(lifetime, from, (Func<Type, object>)activator);
            }
            else
            {
                Func<Type, object> f = (t) =>
                {
                    IDictionary<object, Func<Type, object>> activator;
                    Func<Type, object> act;
                    if (!(container.TryGetValue(t, out activator) && activator.TryGetValue(key, out act)))
                    {
                        var p = t.GetGenericArguments();
                        var ft = to.MakeGenericType(p);
                        ctor = ft.GetConstructors().First();
                        act = (Func<Type, object>)ObjectActivator.GetActivator(ft, container, ctor);
                        if (!container.ContainsKey(t))
                        {
                            container[t] = new ConcurrentDictionary<object, Func<Type, object>>();
                        }

                        container[t][key] = WrappWithLifetimeManager(lifetime, t, act);
                    }

                    return act(t);
                };

                container[from][key] = f;
            }

            return this;
        }

        public IContainer RegisterFactory(Type from, Func<IServiceProvider, object> implementationFactory)
        {
            return RegisterFactory(from, implementationFactory, "");
        }

        public IContainer RegisterFactory(Type from, Func<IServiceProvider, object> implementationFactory, string name)
        {
            return RegisterFactory(from, implementationFactory, name, Lifetime.Transistent);
        }

        public IContainer RegisterFactory(Type from, Func<IServiceProvider, object> implementationFactory, Lifetime lifetime)
        {
            return RegisterFactory(from, implementationFactory, string.Empty, lifetime);
        }

        public IContainer RegisterFactory(Type from, Func<IServiceProvider, object> implementationFactory, string name, Lifetime lifetime)
        {
            InitializeContainer(from);

            container[from][name] = WrappWithLifetimeManager(lifetime, from, (t) => implementationFactory((IServiceProvider)container[typeof(IServiceProvider)][""](t)));
            return this;
        }

        public T Resolve<T>() where T : class
        {
            return (T)Resolve(typeof(T));
        }

        public T Resolve<T>(object key)
            where T : class
        {
            var type = typeof(T);
            return (T)container[type][key](type);
        }

        public object Resolve(Type t)
        {
            return this.Resolve(t, string.Empty);
        }

        public object Resolve(Type t, object key)
        {
            IDictionary<object, Func<Type, object>> df;
            Func<Type, object> f = null;
            if (container.TryGetValue(t, out df) && df.TryGetValue(key, out f))
            {
                return f(t);
            }

            if (t.GetTypeInfo().IsGenericType)
            {
                var gt = t.GetGenericTypeDefinition();
                if (container.TryGetValue(gt, out df) && df.TryGetValue(key, out f))
                {
                    InitializeContainer(t);
                    return f(t);
                }
            }

            if (!t.GetTypeInfo().IsAbstract && !t.GetTypeInfo().IsInterface)
            {
                this.RegisterType(t, t, key);
                return Resolve(t);
            }

            if (t.GetInterfaces().Contains(typeof(IEnumerable)))
            {
                var arg = t.GetGenericArguments().FirstOrDefault();
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

        public static IEnumerable<T> CastList<T>(IEnumerable<object> o)
        {
            return o.Cast<T>().ToList();
        }

        public IEnumerable<T> ResolveAll<T>()
            where T : class
        {
            throw new NotImplementedException();
        }

        private static void InitializeContainer(Type type)
        {
            if (!container.ContainsKey(type))
            {
                container[type] = new ConcurrentDictionary<object, Func<Type, object>>();
            }
        }

        private Func<Type, object> WrappWithLifetimeManager(Lifetime lifetime, Type type, Func<Type, object> activator)
        {
            switch (lifetime)
            {
                case Lifetime.Transistent:
                    return (new TransistentLifetimeManager()).Resolve(type, activator);
                case Lifetime.Singleton:
                    return (new SingletonLifetimeManager()).Resolve(type, activator);
                default:
                    return (new TransistentLifetimeManager()).Resolve(type, activator);
            }
        }
    }
}
