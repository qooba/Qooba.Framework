using System.Threading.Tasks;

namespace Qooba.Framework.Cqrs.Abstractions
{
    public interface ICommandHandler<in TParameter> 
        where TParameter : ICommand
    {
        Task<CommandResult> Execute(TParameter command);
    }
}
