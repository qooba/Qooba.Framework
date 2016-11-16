namespace Qooba.Framework.Expressions.Abstractions
{
    public interface IExpressionHelper
    {
        T CreateInstance<T>(params object[] arguments) where T : class;
    }
}
