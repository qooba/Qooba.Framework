using Qooba.Framework.Specification.Abstractions;
using System;
using System.Linq.Expressions;

namespace Qooba.Framework.Specification
{
    public class AndSpecification<T> : Specification<T>
    {
        private ISpecification<T> _left;

        private ISpecification<T> _right;

        public AndSpecification(ISpecification<T> left, ISpecification<T> right)
        {
            _left = left;
            _right = right;
        }

        public override Expression<Func<T, bool>> IsSatisfiedBy() => _left.IsSatisfiedBy().AndAlso(_right.IsSatisfiedBy());
    }
}
