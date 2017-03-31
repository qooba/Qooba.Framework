using System;
using System.Collections.Generic;

namespace Qooba.Framework.DependencyInjection.Abstractions
{
    public interface IContainer
    {
        bool IsRegistered<T>() where T : class;
        bool IsRegistered<T>(object keyToCheck) where T : class;
        bool IsRegistered(Type typeToCheck);
        bool IsRegistered(Type typeToCheck, object keyToCheck);
        IContainer RegisterInstance<TInterface>(TInterface instance);
        IContainer RegisterInstance<TInterface>(object key, TInterface instance);
        IContainer RegisterInstance<TInterface>(TInterface instance, Lifetime lifetime);
        IContainer RegisterInstance(Type t, object instance);
        IContainer RegisterInstance<TInterface>(object key, TInterface instance, Lifetime lifetime);
        IContainer RegisterInstance(Type t, object instance, Lifetime lifetime);
        IContainer RegisterInstance(Type t, object key, object instance);
        IContainer RegisterType<T>() where T : class;
        IContainer RegisterType<T>(Func<IContainer, T> implementationFactory) where T : class;
        IContainer RegisterType<TFrom, TTo>() where TTo : class, TFrom where TFrom : class;
        IContainer RegisterType<T>(Lifetime lifetime) where T : class;
        IContainer RegisterType<T>(Func<IContainer, T> implementationFactory, Lifetime lifetime) where T : class;
        IContainer RegisterType<TFrom, TTo>(Lifetime lifetime) where TTo : class, TFrom where TFrom : class;
        IContainer RegisterType<TFrom, TTo>(object key) where TTo : class, TFrom;
        IContainer RegisterType<T>(object key) where T : class;
        IContainer RegisterType<T>(object key, Func<IContainer, T> implementationFactory) where T : class;
        IContainer RegisterType(Type t);
        IContainer RegisterType<T>(object key, Lifetime lifetime) where T : class;
        IContainer RegisterType<T>(object key, Func<IContainer, T> implementationFactory, Lifetime lifetime) where T : class;
        IContainer RegisterType<TFrom, TTo>(object key, Lifetime lifetime) where TTo : TFrom;
        IContainer RegisterType(Type t, Lifetime lifetime);
        IContainer RegisterType(Type t, object key);
        IContainer RegisterType(Type from, Type to);
        IContainer RegisterType(Type t, object key, Lifetime lifetime);
        IContainer RegisterType(Type from, Type to, Lifetime lifetime);
        IContainer RegisterType(Type from, Type to, object key);
        T Resolve<T>() where T : class;
        T Resolve<T>(object key) where T : class;
        object Resolve(Type t);
        IEnumerable<T> ResolveAll<T>() where T : class;
    }
}
