using System;
using System.Linq.Expressions;

namespace Qooba.Framework.Specification.Abstractions
{
    public interface ISpecification<T>
    {
        Expression<Func<T, bool>> Predicate { get; }

        IFetchStrategy<T> FetchStrategy { get; set; }

        Expression<Func<T, bool>> IsSatisfiedBy();

        ISpecification<T> And(ISpecification<T> specification);

        ISpecification<T> Or(ISpecification<T> specification);

        ISpecification<T> Not();
        
    }
}
