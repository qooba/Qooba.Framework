namespace Qooba.Framework.Specification.Abstractions
{
    public interface IFetchStrategyFactory
    {
        IFetchStrategy<T> Create<T>();
    }
}
