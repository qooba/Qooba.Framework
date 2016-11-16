using System.Threading.Tasks;

namespace Qooba.Framework.Cqrs.Abstractions
{
    public interface IQueryHandler<TParameter,TResult>
        where TParameter : IQuery
        where TResult : IQueryResult
    {
        Task<TResult> Retrieve(TParameter query);
    }
}
