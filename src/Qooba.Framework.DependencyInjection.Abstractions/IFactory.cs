namespace Qooba.Framework.DependencyInjection.Abstractions
{
    public interface IFactory<T>
        where T : class
    {
        T Create();
    }

    public interface IFactory
    {
        T Create<T>() where T : class;
    }
}
