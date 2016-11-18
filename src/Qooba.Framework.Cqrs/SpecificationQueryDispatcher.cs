using Qooba.Framework.Cqrs.Abstractions;
using Qooba.Framework.DependencyInjection.Abstractions;
using Qooba.Framework.Mapping.Abstractions;
using Qooba.Framework.Specification.Abstractions;
using Qooba.Framework.UnitOfWork.Abstractions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Qooba.Framework.Cqrs
{
    public class SpecificationQueryDispatcher : ISpecificationQueryDispatcher
    {
        private readonly IFactory factory;

        private readonly IMapper mapper;

        public SpecificationQueryDispatcher(IFactory factory, IMapper mapper)
        {
            this.factory = factory;
            this.mapper = mapper;
        }

        public async Task<QueryResult<List<TParameter>>> Dispatch<TDtoParameter, TParameter>(ISpecification<TDtoParameter> specification)
            where TParameter : class
            where TDtoParameter : class
        {
            var query = new QueryFilterParameter<TDtoParameter>
            {
                Specification = specification
            };

            var result = await Task.Run(() => this.factory.Create<IRepositoryQueries<TDtoParameter>>().FilterAsync(specification));
            var data = result.Select(p => this.mapper.Map<TDtoParameter, TParameter>(p)).ToList();
            return new QueryResult<List<TParameter>>
            {
                Data = data,
                Success = true
            };
        }
    }
}
