using Qooba.Framework.Cqrs.Abstractions;
using Qooba.Framework.DependencyInjection.Abstractions;
using System.Threading.Tasks;

namespace Qooba.Framework.Cqrs
{
    public class QueryDispatcher : IQueryDispatcher
    {
        private readonly IFactory factory;

        public QueryDispatcher(IFactory factory)
        {
            this.factory = factory;
        }

        public async Task<TResult> Dispatch<TParameter, TResult>(TParameter query)
            where TParameter : IQuery
            where TResult : IQueryResult
        {
            return await this.factory.Create<IQueryHandler<TParameter, TResult>>().Retrieve(query);
        }
    }
}
