using Qooba.Framework.Specification.Abstractions;

namespace Qooba.Framework.Cqrs.Abstractions
{
    public class QueryFilterParameter<TParameter> : IQuery
        where TParameter : class
    {
        public ISpecification<TParameter> Specification { get; set; }
    }
}