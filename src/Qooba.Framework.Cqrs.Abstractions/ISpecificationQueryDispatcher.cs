using Qooba.Framework.Specification.Abstractions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Qooba.Framework.Cqrs.Abstractions
{
    public interface ISpecificationQueryDispatcher
    {
        Task<QueryResult<List<TParameter>>> Dispatch<TDtoParameter, TParameter>(ISpecification<TDtoParameter> specification)
            where TParameter : class
            where TDtoParameter : class;
    }
}
