using System.Threading.Tasks;

namespace Qooba.Framework.Cqrs.Abstractions
{
    public interface IEventDispatcher
    {
        Task Dispatch<TParameter>(TParameter command) where TParameter : IEvent;
    }
}
