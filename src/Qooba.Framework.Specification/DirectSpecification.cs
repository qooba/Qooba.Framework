using System;
using System.Linq.Expressions;

namespace Qooba.Framework.Specification
{
    public class DirectSpecification<T>: Specification<T>
    {
        private readonly Expression<Func<T, bool>> _criteria;

        public DirectSpecification(Expression<Func<T,bool>> criteria)
        {
            if(criteria == null)
            {
                throw new ArgumentNullException("criteria");
            }

            _criteria = criteria;
        }

        public override Expression<Func<T, bool>> IsSatisfiedBy() => _criteria;
    }
}
