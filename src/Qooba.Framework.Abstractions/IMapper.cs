namespace Qooba.Framework.Abstractions
{
    public interface IMapper
    {
        TOut Map<TIn, TOut>(TIn input);
    }
}
