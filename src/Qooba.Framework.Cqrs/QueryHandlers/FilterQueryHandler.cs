using System.Threading.Tasks;
using Qooba.Framework.Cqrs.Abstractions;
using Qooba.Framework.UnitOfWork.Abstractions;
using System.Collections.Generic;
using System.Linq;

namespace Qooba.Framework.Cqrs.QueryHandlers
{
    public class FilterQueryHandler<TParameter> : IQueryHandler<QueryFilterParameter<TParameter>, QueryResult<List<TParameter>>>
        where TParameter : class
    {
        private IRepository<TParameter> repository;

        public FilterQueryHandler(IRepository<TParameter> repository)
        {
            this.repository = repository;
        }
        
        public async Task<QueryResult<List<TParameter>>> Retrieve(QueryFilterParameter<TParameter> query)
        {
            return new QueryResult<List<TParameter>>
            {
                Data = await Task.Run(() => query.Specification != null ? this.repository.Filter(query.Specification).ToList() : this.repository.All().ToList()),
                Success = true
            };
        }
    }
}
