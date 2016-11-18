using System.Threading.Tasks;
using Qooba.Framework.Cqrs.Abstractions;
using Qooba.Framework.UnitOfWork.Abstractions;

namespace Qooba.Framework.Cqrs.QueryHandlers
{
    public abstract class BaseQueryHandler<TParameter, TResult> : BaseHandler, IQueryHandler<TParameter, TResult>
        where TParameter : class, IQuery
        where TResult : IQueryResult
    {
        public BaseQueryHandler(IRepositoryQueries<TParameter> repository)
        {
            this.Repository = repository;
        }

        public IRepositoryQueries<TParameter> Repository { get; private set; }

        public abstract Task<TResult> Retrieve(TParameter query);
    }
}
