using System.Threading.Tasks;

namespace Qooba.Framework.Cqrs.Abstractions
{
    public interface IQueryDispatcher
    {
        Task<TResult> Dispatch<TParameter, TResult>(TParameter query)
            where TParameter : IQuery
            where TResult : IQueryResult;
    }
}
