using System.Collections.Generic;

namespace Qooba.Framework.Abstractions
{
    public interface IFactory<T>
        where T : class
    {
        T Create();

        T Create(string key);

        IList<T> CreateAll();
    }

    public interface IFactory
    {
        T Create<T>() where T : class;

        T Create<T>(string key) where T : class;

        IList<T> CreateAll<T>() where T : class;
    }
}
