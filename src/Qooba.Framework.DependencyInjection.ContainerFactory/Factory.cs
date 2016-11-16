using Qooba.Framework.DependencyInjection.Abstractions;

namespace Qooba.Framework.DependencyInjection.ContainerFactory
{
    public class Factory<T>: IFactory<T>
        where T : class
    {
        public T Create()
        {
            return ContainerManager.Current.Resolve<T>();
        }
    }

    public class Factory : IFactory
    {
        public T Create<T>()
            where T : class
        {
            return ContainerManager.Current.Resolve<T>();
        }
    }
}
