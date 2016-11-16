using System.Threading.Tasks;

namespace Qooba.Framework.Cqrs.Abstractions
{
    public interface IEventDispatcher
    {
        Task<EventResult> Dispatch<TParameter>(TParameter command) where TParameter : IEvent;
    }
}
