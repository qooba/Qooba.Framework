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

        public IContainer RegisterInstance<TInterface>(TInterface instance) => this.RegisterInstance(string.Empty, instance);

        public IContainer RegisterInstance<TInterface>(object key, TInterface instance)
        {
            var type = typeof(TInterface);
            InitializeContainer(type);

            container[type][key] = (t) => instance;
            return this;
        }

        public IContainer RegisterInstance<TInterface>(TInterface instance, Lifetime lifetime) => this.RegisterType(typeof(TInterface), instance.GetType(), lifetime);

        public IContainer RegisterInstance(Type t, object instance) => this.RegisterInstance(t, instance, string.Empty);


        public IContainer RegisterInstance(Type t, object instance, object key)
        {
            InitializeContainer(t);

            container[t][key] = (x) => instance;
            return this;
        }

        public IContainer RegisterInstance<TInterface>(object key, TInterface instance, Lifetime lifetime) => this.RegisterType(typeof(TInterface), instance.GetType(), key, lifetime);

        public IContainer RegisterInstance(Type t, object instance, Lifetime lifetime) => this.RegisterType(t, instance.GetType(), lifetime);

        public IContainer RegisterType<T>()
            where T : class => this.RegisterType<T, T>(string.Empty);


        public IContainer RegisterType<T>(Lifetime lifetime)
            where T : class => this.RegisterType<T, T>(string.Empty, lifetime);

        public IContainer RegisterType<TFrom, TTo>()
            where TTo : class, TFrom
            where TFrom : class => this.RegisterType<TFrom, TTo>(string.Empty);


        public IContainer RegisterType<TFrom, TTo>(Lifetime lifetime)
            where TTo : class, TFrom
            where TFrom : class => this.RegisterType(typeof(TFrom), typeof(TTo), lifetime);


        public IContainer RegisterType<TFrom, TTo>(object key) where TTo : class, TFrom
        {
            var type = typeof(TFrom);
            InitializeContainer(type);

            return RegisterType(typeof(TFrom), typeof(TTo), key);
        }

        public IContainer RegisterType<T>(object key)
            where T : class => this.RegisterType<T, T>(key);


        public IContainer RegisterType(Type t) => RegisterType(t, t);


        public IContainer RegisterType<T>(object key, Lifetime lifetime)
            where T : class
        {
            var type = typeof(T);
            return RegisterType(type, type, key);
        }

        public IContainer RegisterType<TFrom, TTo>(object key, Lifetime lifetime) where TTo : TFrom => RegisterType(typeof(TFrom), typeof(TTo), key, lifetime);
        
        public IContainer RegisterType(Type t, Lifetime lifetime) => RegisterType(t, t, lifetime);
        
        public IContainer RegisterType(Type t, object key) => RegisterType(t, t, key);
        
        public IContainer RegisterType(Type from, Type to) => RegisterType(from, to, string.Empty);
        
        public IContainer RegisterType(Type t, object key, Lifetime lifetime) => RegisterType(t, t, key, lifetime);
        
        public IContainer RegisterType(Type from, Type to, Lifetime lifetime) => this.RegisterType(from, to, string.Empty, lifetime);
        
        public IContainer RegisterType(Type from, Type to, object key) => this.RegisterType(from, to, key, Lifetime.Transistent);
        
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

        public IContainer RegisterFactory(Type from, Func<IServiceProvider, object> implementationFactory) => RegisterFactory(from, implementationFactory, "");
        
        public IContainer RegisterFactory(Type from, Func<IServiceProvider, object> implementationFactory, string name) => RegisterFactory(from, implementationFactory, name, Lifetime.Transistent);
        
        public IContainer RegisterFactory(Type from, Func<IServiceProvider, object> implementationFactory, Lifetime lifetime) => RegisterFactory(from, implementationFactory, string.Empty, lifetime);
        
        public IContainer RegisterFactory(Type from, Func<IServiceProvider, object> implementationFactory, string name, Lifetime lifetime)
        {
            InitializeContainer(from);

            container[from][name] = WrappWithLifetimeManager(lifetime, from, (t) => implementationFactory((IServiceProvider)container[typeof(IServiceProvider)][""](t)));
            return this;
        }

        public IContainer RegisterType<T>(Func<IContainer, T> implementationFactory) where T : class => this.RegisterType(implementationFactory, Lifetime.Transistent);

        public IContainer RegisterType<T>(Func<IContainer, T> implementationFactory, Lifetime lifetime) where T : class => this.RegisterType(string.Empty, implementationFactory, lifetime);

        public IContainer RegisterType<T>(object key, Func<IContainer, T> implementationFactory) where T : class => this.RegisterType(key, implementationFactory, Lifetime.Transistent);

        public IContainer RegisterType<T>(object key, Func<IContainer, T> implementationFactory, Lifetime lifetime) where T : class
        {
            var type = typeof(T);
            container[type][key] = WrappWithLifetimeManager(lifetime, type, (t) => implementationFactory(this));
            return this;
        }

        public T Resolve<T>() where T : class => (T)Resolve(typeof(T));
        
        public T Resolve<T>(object key) where T : class => (T)this.Resolve(typeof(T), key);

        public object Resolve(Type t) => this.Resolve(t, string.Empty);
        
        public object Resolve(Type t, object key)
        {
            if (container.TryGetValue(t, out IDictionary<object, Func<Type, object>> df) && df.TryGetValue(key, out Func<Type, object> f))
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

        public static IEnumerable<T> CastList<T>(IEnumerable<object> o) => o.Cast<T>().ToList();
        
        public IEnumerable<T> ResolveAll<T>()
            where T : class
        {
            var type = typeof(T);
            var keys = container[type].Keys;
            foreach (var key in keys)
            {
                yield return (T)this.Resolve(type, key);
            }
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
