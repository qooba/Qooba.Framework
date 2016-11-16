using Qooba.Framework.DependencyInjection.Abstractions;
using Qooba.Framework.Specification.Abstractions;
using System;
using System.Linq.Expressions;

namespace Qooba.Framework.Specification
{
    public class SpecificationFactory: ISpecificationFactory
    {
        private readonly IFactory factory;
        public SpecificationFactory(IFactory factory)
        {
            this.factory = factory;
        }

        public ISpecification<T> Create<T>(Expression<Func<T, bool>> criteria, Func<IFetchStrategy<T>, IFetchStrategy<T>> fetchStrategy = null)
        {
            var fs = this.factory.Create<IFetchStrategy<T>>();
            if(fetchStrategy != null)
            {
                fs = fetchStrategy(fs);
            }

            return new DirectSpecification<T>(criteria)
            {
                FetchStrategy = fs
            };
        }
    }
}
