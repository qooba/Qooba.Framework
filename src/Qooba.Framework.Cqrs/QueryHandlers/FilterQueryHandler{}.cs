using System.Threading.Tasks;
using Qooba.Framework.Cqrs.Abstractions;
using Qooba.Framework.UnitOfWork.Abstractions;
using System.Collections.Generic;
using System.Linq;

namespace Qooba.Framework.Cqrs.QueryHandlers
{
    public class FilterQueryHandler<TParameter, TResult> : IQueryHandler<QueryFilterParameter<TParameter, TResult>, QueryResult<List<TResult>>>
        where TParameter : class
        where TResult : class
    {
        private IRepositoryQueries<TParameter> repository;

        public FilterQueryHandler(IRepositoryQueries<TParameter> repository)
        {
            this.repository = repository;
        }

        public async Task<QueryResult<List<TResult>>> Retrieve(QueryFilterParameter<TParameter, TResult> query)
        {
            return new QueryResult<List<TResult>>
            {
                Data = (await Task.Run(() => query.Specification != null ? this.repository.FilterAsync(query.Specification, query.Selector) : this.repository.AllAsync(query.Selector))).ToList(),
                Success = true
            };
        }
    }
}
