using Qooba.Framework.Specification.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Qooba.Framework.Specification
{
    public class FetchStrategy<T> : IFetchStrategy<T>
    {
        private readonly IList<Expression<Func<T, object>>> _props;

        public FetchStrategy()
        {
            _props = new List<Expression<Func<T, object>>>();
        }

        public IFetchStrategy<T> Include(Expression<Func<T, object>> path)
        {
            _props.Add(path);
            return this;
        }

        public IFetchStrategy<T> Include<TProperty>(Expression<Func<T, TProperty>> navigationPropertyPath)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> IncludedPaths
        {
            get { return _props.Select(x => x.ToPropertyName()); }
        }

        public IEnumerable<Expression<Func<T, object>>> Includes
        {
            get { return _props; }
        }

        public IEnumerable<Func<IQueryable<T>, IQueryable<T>>> IncludedNavigationPaths
        {
            get
            {
                throw new NotImplementedException();
            }
        }
    }
}
