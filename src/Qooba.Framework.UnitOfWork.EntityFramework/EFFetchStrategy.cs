using Qooba.Framework.Specification.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Qooba.Framework.UnitOfWork.EntityFramework
{
    public class EFFetchStrategy<T> : IFetchStrategy<T>
        where T : class
    {
        private readonly IList<Func<IQueryable<T>, IQueryable<T>>> _propsInclude;
        
        public EFFetchStrategy()
        {
            _propsInclude = new List<Func<IQueryable<T>, IQueryable<T>>>();
        }

        public IFetchStrategy<T> Include(Expression<Func<T, object>> path)
        {
            return this;
        }

        public IFetchStrategy<T> Include<TProperty>(Expression<Func<T, TProperty>> navigationPropertyPath)
        {
            Func<IQueryable<T>, IQueryable<T>> includeExpression = x => x.Include(navigationPropertyPath);
            _propsInclude.Add(includeExpression);
            return this;
        }

        public IEnumerable<Func<IQueryable<T>, IQueryable<T>>> IncludedNavigationPaths
        {
            get { return this._propsInclude; }
        }

        public IEnumerable<string> IncludedPaths
        {
            get { return null; }
        }

        public IEnumerable<Expression<Func<T, object>>> Includes
        {
            get { return null; }
        }
    }
}
