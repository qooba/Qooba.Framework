using System.Threading.Tasks;
using Qooba.Framework.Cqrs.Abstractions;
using Qooba.Framework.UnitOfWork.Abstractions;

namespace Qooba.Framework.Cqrs.QueryHandlers
{
    public class CountQueryHandler<TParameter> : IQueryHandler<QueryFilterParameter<TParameter>, QueryResult<int>>
        where TParameter : class
    {
        private IRepository<TParameter> repository;

        public CountQueryHandler(IRepository<TParameter> repository)
        {
            this.repository = repository;
        }
        
        public async Task<QueryResult<int>> Retrieve(QueryFilterParameter<TParameter> query)
        {
            return new QueryResult<int>
            {
                Data = query.Specification != null ? await this.repository.CountAsync(query.Specification) : await this.repository.CountAsync(),
                Success = true
            };
        }
    }
}
