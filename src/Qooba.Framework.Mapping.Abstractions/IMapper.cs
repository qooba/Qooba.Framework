namespace Qooba.Framework.Mapping.Abstractions
{
    public interface IMapper
    {
        TOut Map<TIn, TOut>(TIn input);
    }
}
