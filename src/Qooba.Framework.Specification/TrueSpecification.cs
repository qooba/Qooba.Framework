using System;
using System.Linq.Expressions;

namespace Qooba.Framework.Specification
{
    public class TrueSpecification<T> : Specification<T>
    {
        public override Expression<Func<T, bool>> IsSatisfiedBy() => t => true;
    }
}
