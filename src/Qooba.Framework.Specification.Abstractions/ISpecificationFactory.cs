using System;
using System.Linq.Expressions;

namespace Qooba.Framework.Specification.Abstractions
{
    public interface ISpecificationFactory
    {
        ISpecification<T> Create<T>(Expression<Func<T, bool>> criteria, Func<IFetchStrategy<T>, IFetchStrategy<T>> fetchStrategy = null);
    }
}
