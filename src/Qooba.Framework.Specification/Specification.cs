using Qooba.Framework.Specification.Abstractions;
using System;
using System.Linq.Expressions;

namespace Qooba.Framework.Specification
{
    public class Specification<T> : ISpecification<T>
    {
        protected IFetchStrategy<T> _fetchStrategy;

        protected Expression<Func<T, bool>> _predicate;

        protected readonly Expression<Func<T, bool>> Default = x => true;

        public Specification()
        {
            _predicate = Default;
            _fetchStrategy = new FetchStrategy<T>();

        }

        public Expression<Func<T, bool>> Predicate => _predicate;

        public IFetchStrategy<T> FetchStrategy
        {
            get { return _fetchStrategy; }
            set { _fetchStrategy = value; }
        }

        public virtual Expression<Func<T, bool>> IsSatisfiedBy() => Expression.Lambda<Func<T, bool>>(_predicate.Body, _predicate.Parameters);

        public ISpecification<T> And(ISpecification<T> specification) => new AndSpecification<T>(this, specification);

        public ISpecification<T> Or(ISpecification<T> specification) => new OrSpecification<T>(this, specification);

        public ISpecification<T> Not() => new NotSpecification<T>(this);
    }
}
