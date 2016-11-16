using System.Threading.Tasks;

namespace Qooba.Framework.Cqrs.Abstractions
{
    public interface ICommandDispatcher
    {
        Task<CommandResult> Dispatch<TParameter>(TParameter command) where TParameter : ICommand;
    }
}
