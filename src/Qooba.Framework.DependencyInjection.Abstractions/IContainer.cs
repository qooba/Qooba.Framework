using Qooba.Framework.Abstractions;
using System;
using System.Collections.Generic;

namespace Qooba.Framework.DependencyInjection.Abstractions
{
    public interface IContainer : IServiceManager
    {
        bool IsRegistered(Type typeToCheck, object keyToCheck);

        IContainer RegisterInstance(object key, Type from, object instance);

        IContainer RegisterType(object key, Type from, Type to, Lifetime lifetime);

        IContainer RegisterFactory(object key, Type from, Func<IContainer, object> implementationFactory, Lifetime lifetime);

        object Resolve(object key, Type from);

        IEnumerable<object> ResolveAll(Type from);
    }
}
