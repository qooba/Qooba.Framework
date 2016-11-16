using System;
using System.Collections.Generic;

namespace Qooba.Framework.DependencyInjection.Abstractions
{
    public interface IContainer
    {
        T BuildUp<T>(T existing);
        T BuildUp<T>(T existing, string name);
        object BuildUp(Type t, object existing);
        bool IsRegistered<T>();
        bool IsRegistered<T>(string nameToCheck);
        bool IsRegistered(Type typeToCheck);
        bool IsRegistered(Type typeToCheck, string nameToCheck);
        IContainer RegisterInstance<TInterface>(TInterface instance);
        IContainer RegisterInstance<TInterface>(string name, TInterface instance);
        IContainer RegisterInstance<TInterface>(TInterface instance, Lifetime lifetime);
        IContainer RegisterInstance(Type t, object instance);
        IContainer RegisterInstance<TInterface>(string name, TInterface instance, Lifetime lifetime);
        IContainer RegisterInstance(Type t, object instance, Lifetime lifetime);
        IContainer RegisterInstance(Type t, string name, object instance);
        IContainer RegisterType<T>();
        IContainer RegisterType<TFrom, TTo>() where TTo : class, TFrom where TFrom : class;
        IContainer RegisterType<T>(Lifetime lifetime);
        IContainer RegisterType<TFrom, TTo>(Lifetime lifetime) where TTo : class, TFrom where TFrom : class;
        IContainer RegisterType<TFrom, TTo>(string name) where TTo : class, TFrom;
        IContainer RegisterType<T>(string name);
        IContainer RegisterType(Type t);
        IContainer RegisterType<T>(string name, Lifetime lifetime);
        IContainer RegisterType<TFrom, TTo>(string name, Lifetime lifetime) where TTo : TFrom;
        IContainer RegisterType(Type t, Lifetime lifetime);
        IContainer RegisterType(Type t, string name);
        IContainer RegisterType(Type from, Type to);
        IContainer RegisterType(Type t, string name, Lifetime lifetime);
        IContainer RegisterType(Type from, Type to, Lifetime lifetime);
        IContainer RegisterType(Type from, Type to, string name);
        T Resolve<T>() where T : class;
        T Resolve<T>(string name);
        object Resolve(Type t);
        IEnumerable<T> ResolveAll<T>();
        void Populate(object services);
    }
}
