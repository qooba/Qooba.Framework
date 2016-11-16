using System.Threading.Tasks;
using Qooba.Framework.Cqrs.Abstractions;
using Qooba.Framework.UnitOfWork.Abstractions;

namespace Qooba.Framework.Cqrs.CommandHandlers
{
    public class UpdateCommandHandler<TCommand> : BaseCommandHandler<TCommand>
        where TCommand : class, ICommand 
    {
        public UpdateCommandHandler(IRepository<TCommand> repository)
            : base(repository)
        {
        }

        public override async Task<CommandResult> Execute(TCommand command)
        {
            this.Repository.Add(command);
            await this.Repository.UnitOfWork.CommitAsync();
            return this.CreateSuccessResult();
        }
    }
}
