using System.Threading.Tasks;
using Qooba.Framework.Cqrs.Abstractions;
using Qooba.Framework.UnitOfWork.Abstractions;

namespace Qooba.Framework.Cqrs.QueryHandlers
{
    public class AnyQueryHandler<TParameter> : IQueryHandler<QueryFilterParameter<TParameter>, QueryResult<bool>>
        where TParameter : class
    {
        private IRepositoryQueries<TParameter> repository;

        public AnyQueryHandler(IRepositoryQueries<TParameter> repository)
        {
            this.repository = repository;
        }
        
        public async Task<QueryResult<bool>> Retrieve(QueryFilterParameter<TParameter> query)
        {
            return new QueryResult<bool>
            {
                Data = query.Specification != null ? await this.repository.AnyAsync(query.Specification) : await this.repository.AnyAsync(),
                Success = true
            };
        }
    }
}
