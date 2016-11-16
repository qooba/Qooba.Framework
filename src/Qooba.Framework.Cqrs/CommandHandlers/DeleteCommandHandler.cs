using System.Threading.Tasks;
using Qooba.Framework.Cqrs.Abstractions;
using Qooba.Framework.UnitOfWork.Abstractions;

namespace Qooba.Framework.Cqrs.CommandHandlers
{
    public class DeleteCommandHandler<TCommand> : BaseCommandHandler<TCommand>
        where TCommand : class, ICommand 
    {
        public DeleteCommandHandler(IRepository<TCommand> repository)
            : base(repository)
        {
        }

        public override async Task<CommandResult> Execute(TCommand command)
        {
            this.Repository.Remove(command);
            await this.Repository.UnitOfWork.CommitAsync();
            return this.CreateSuccessResult();
        }
    }
}
