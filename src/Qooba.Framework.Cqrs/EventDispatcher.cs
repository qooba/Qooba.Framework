using Qooba.Framework.Cqrs.Abstractions;
using Qooba.Framework.DependencyInjection.Abstractions;
using System.Threading.Tasks;
using System.Linq;

namespace Qooba.Framework.Cqrs
{
    public class EventDispatcher : IEventDispatcher
    {
        private readonly IFactory factory;

        public EventDispatcher(IFactory factory)
        {
            this.factory = factory;
        }
        
        public async Task Dispatch<TParameter>(TParameter command) where TParameter : IEvent
        {
            this.factory.CreateAll<IEventHandler<TParameter>>().Select(x => x.Execute(command));
        }
    }
}
