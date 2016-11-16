using System.Threading.Tasks;
using Qooba.Framework.Cqrs.Abstractions;
using Qooba.Framework.UnitOfWork.Abstractions;

namespace Qooba.Framework.Cqrs.QueryHandlers
{
    public class SingleQueryHandler<TParameter> : IQueryHandler<QueryFilterParameter<TParameter>, QueryResult<TParameter>>
        where TParameter : class
    {
        private IRepository<TParameter> repository;

        public SingleQueryHandler(IRepository<TParameter> repository)
        {
            this.repository = repository;
        }

        public async Task<QueryResult<TParameter>> Retrieve(QueryFilterParameter<TParameter> query)
        {
            return new QueryResult<TParameter>
            {
                Data = await this.repository.SingleAsync(query.Specification),
                Success = true
            };
        }
    }
}
