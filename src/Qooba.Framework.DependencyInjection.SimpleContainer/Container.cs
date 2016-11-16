//using Microsoft.Extensions.DependencyInjection;
using Qooba.Framework.DependencyInjection.Abstractions;
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
        private static IDictionary<Type, IDictionary<string, Func<Type, object>>> container = new ConcurrentDictionary<Type, IDictionary<string, Func<Type, object>>>();

        private static IDictionary<Type, Func<IEnumerable<object>, object>> castingExpressions = new ConcurrentDictionary<Type, Func<IEnumerable<object>, object>>();

        public T GetType<T>()
        {
            return default(T);
            //return ObjectActivator.GetType<T>(container);
        }

        public T BuildUp<T>(T existing)
        {
            throw new NotImplementedException();
        }

        public T BuildUp<T>(T existing, string name)
        {
            throw new NotImplementedException();
        }

        public object BuildUp(Type t, object existing)
        {
            throw new NotImplementedException();
        }

        public bool IsRegistered<T>()
        {
            throw new NotImplementedException();
        }

        public bool IsRegistered<T>(string nameToCheck)
        {
            throw new NotImplementedException();
        }

        public bool IsRegistered(Type typeToCheck)
        {
            throw new NotImplementedException();
        }

        public bool IsRegistered(Type typeToCheck, string nameToCheck)
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

        public IContainer RegisterInstance<TInterface>(string name, TInterface instance)
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

        public IContainer RegisterInstance(Type t, object instance, string name)
        {
            InitializeContainer(t);

            container[t][name] = (x) => instance;
            return this;
        }

        public IContainer RegisterInstance<TInterface>(string name, TInterface instance, Lifetime lifetime)
        {
            throw new NotImplementedException();
        }

        public IContainer RegisterInstance(Type t, object instance, Lifetime lifetime)
        {
            throw new NotImplementedException();
        }

        public IContainer RegisterInstance(Type t, string name, object instance)
        {
            throw new NotImplementedException();
        }

        public IContainer RegisterType<T>()
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

        public IContainer RegisterType<TFrom, TTo>(string name) where TTo : class, TFrom
        {
            var type = typeof(TFrom);
            InitializeContainer(type);

            return RegisterType(typeof(TFrom), typeof(TTo), name);
        }

        public IContainer RegisterType<T>(string name)
        {
            throw new NotImplementedException();
        }

        public IContainer RegisterType(Type t)
        {
            throw new NotImplementedException();
        }

        public IContainer RegisterType<T>(string name, Lifetime lifetime)
        {
            throw new NotImplementedException();
        }

        public IContainer RegisterType<TFrom, TTo>(string name, Lifetime lifetime) where TTo : TFrom
        {
            throw new NotImplementedException();
        }

        public IContainer RegisterType(Type t, Lifetime lifetime)
        {
            throw new NotImplementedException();
        }

        public IContainer RegisterType(Type t, string name)
        {
            throw new NotImplementedException();
        }

        public IContainer RegisterType(Type from, Type to)
        {
            return RegisterType(from, to, "");
        }

        public IContainer RegisterType(Type t, string name, Lifetime lifetime)
        {
            throw new NotImplementedException();
        }

        public IContainer RegisterType(Type from, Type to, Lifetime lifetime)
        {
            return this.RegisterType(from, to, string.Empty, lifetime);
        }

        public IContainer RegisterType(Type from, Type to, string name)
        {
            return this.RegisterType(from, to, name, Lifetime.Transistent);
        }

        public IContainer RegisterType(Type from, Type to, string name, Lifetime lifetime)
        {
            InitializeContainer(from);
            var ctor = to.GetConstructors().First();
            if (!from.GetTypeInfo().IsGenericTypeDefinition)
            {

                var activator = ObjectActivator.GetActivator(to, container, ctor);
                container[from][name] = WrappWithLifetimeManager(lifetime, from, (Func<Type, object>)activator);
            }
            else
            {
                Func<Type, object> f = (t) =>
                {
                    IDictionary<string, Func<Type, object>> activator;
                    Func<Type, object> act;
                    if (!(container.TryGetValue(t, out activator) && activator.TryGetValue(name, out act)))
                    {
                        var p = t.GetGenericArguments();
                        var ft = to.MakeGenericType(p);
                        ctor = ft.GetConstructors().First();
                        act = (Func<Type, object>)ObjectActivator.GetActivator(ft, container, ctor);
                        if (!container.ContainsKey(t))
                        {
                            container[t] = new ConcurrentDictionary<string, Func<Type, object>>();
                        }

                        container[t][name] = WrappWithLifetimeManager(lifetime, t, act);
                    }

                    return act(t);
                };

                container[from][name] = f;
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

        public T Resolve<T>(string name)
        {
            var type = typeof(T);
            return (T)container[type][name](type);
        }

        public object Resolve(Type t)
        {
            return this.Resolve(t, string.Empty);
        }

        public object Resolve(Type t, string name)
        {
            IDictionary<string, Func<Type, object>> df;
            Func<Type, object> f = null;
            if (container.TryGetValue(t, out df) && df.TryGetValue(name, out f))
            {
                return f(t);
            }

            if (t.GetTypeInfo().IsGenericType)
            {
                var gt = t.GetGenericTypeDefinition();
                if (container.TryGetValue(gt, out df) && df.TryGetValue(name, out f))
                {
                    InitializeContainer(t);
                    return f(t);
                }
            }

            if (!t.GetTypeInfo().IsAbstract && !t.GetTypeInfo().IsInterface)
            {
                this.RegisterType(t, t, name);
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
                        castingExpression = Expression.Lambda<Func<IEnumerable<object>,object>>(Expression.Call(null, meth, p), p).Compile();
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

        public void Populate(object services)
        {
            //var servicesCollection = services as IServiceCollection;
            //if (servicesCollection != null)
            //{
            //    this.RegisterInstance(servicesCollection);
            //    this.RegisterType<IServiceProvider, ContainerServiceProvider>();
            //    this.RegisterType<IServiceScopeFactory, ContainerServiceScopeFactory>();

            //    foreach (var descriptor in servicesCollection)
            //    {
            //        this.RegisterDescriptor(descriptor);
            //    }
            //}
        }

        //private void RegisterDescriptor(ServiceDescriptor descriptor)
        //{
        //    if (descriptor.ImplementationType != null)
        //    {
        //        this.RegisterType(descriptor.ServiceType, descriptor.ImplementationType, MapLifetime(descriptor.Lifetime));
        //    }

        //    if (descriptor.ImplementationFactory != null)
        //    {
        //        this.RegisterFactory(descriptor.ServiceType, descriptor.ImplementationFactory, MapLifetime(descriptor.Lifetime));
        //    }

        //    if (descriptor.ImplementationInstance != null)
        //    {
        //        this.RegisterInstance(descriptor.ServiceType, descriptor.ImplementationInstance);
        //    }
        //}

        public IEnumerable<T> ResolveAll<T>()
        {
            throw new NotImplementedException();
        }

        private static void InitializeContainer(Type type)
        {
            if (!container.ContainsKey(type))
            {
                container[type] = new ConcurrentDictionary<string, Func<Type, object>>();
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

        //private Lifetime MapLifetime(ServiceLifetime lifetime)
        //{
        //    switch (lifetime)
        //    {
        //        case ServiceLifetime.Transient:
        //            return Lifetime.Transistent;
        //        case ServiceLifetime.Singleton:
        //            return Lifetime.Singleton;
        //        case ServiceLifetime.Scoped:
        //            return Lifetime.Transistent;
        //        default:
        //            return Lifetime.Transistent;
        //    }
        //}
    }
}
