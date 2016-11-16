using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Qooba.Framework.Specification.Abstractions
{
    public interface IFetchStrategy<T>
    {
        //IFetchStrategy<T> Include(Expression<Func<T, object>> path);

        IFetchStrategy<T> Include<TProperty>(Expression<Func<T, TProperty>> navigationPropertyPath);

        //IEnumerable<string> IncludedPaths { get; }

        //IEnumerable<Expression<Func<T, object>>> Includes { get; }

        IEnumerable<Func<IQueryable<T>, IQueryable<T>>> IncludedNavigationPaths { get; }
    }
}
