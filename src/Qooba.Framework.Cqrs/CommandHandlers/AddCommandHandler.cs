using System.Threading.Tasks;
using Qooba.Framework.Cqrs.Abstractions;
using Qooba.Framework.UnitOfWork.Abstractions;

namespace Qooba.Framework.Cqrs.CommandHandlers
{
    public class AddCommandHandler<TCommand> : BaseCommandHandler<TCommand>
        where TCommand : class, ICommand 
    {
        public AddCommandHandler(IRepositoryCommands<TCommand> repository)
            : base(repository)
        {
        }

        public override async Task<CommandResult> Execute(TCommand command)
        {
            await this.Repository.AddAndCommitAsync(command);
            return this.CreateSuccessResult();
        }
    }
}
