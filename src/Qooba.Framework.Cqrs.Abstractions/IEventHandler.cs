using System.Threading.Tasks;

namespace Qooba.Framework.Cqrs.Abstractions
{
    public interface IEventHandler<in TParameter> 
        where TParameter : IEvent
    {
        Task<EventResult> Execute(TParameter command);
    }
}
