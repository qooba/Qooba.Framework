using System.Collections.Generic;

namespace Qooba.Framework.DependencyInjection.Abstractions
{
    public interface IFactory<T>
        where T : class
    {
        T Create();

        IList<T> CreateAll();
    }

    public interface IFactory
    {
        T Create<T>() where T : class;

        IList<T> CreateAll<T>() where T : class;
    }
}
