using Qooba.Framework.DependencyInjection.Abstractions;
using System.Collections.Generic;
using System.Linq;

namespace Qooba.Framework.DependencyInjection.ContainerFactory
{
    public class Factory<T>: IFactory<T>
        where T : class
    {
        public T Create()
        {
            return ContainerManager.Current.Resolve<T>();
        }

        public IList<T> CreateAll()
        {
            return ContainerManager.Current.ResolveAll<T>().ToList();
        }
    }

    public class Factory : IFactory
    {
        public T Create<T>()
            where T : class
        {
            return ContainerManager.Current.Resolve<T>();
        }

        public IList<T> CreateAll<T>()
            where T : class
        {
            return ContainerManager.Current.ResolveAll<T>().ToList();
        }
    }
}
