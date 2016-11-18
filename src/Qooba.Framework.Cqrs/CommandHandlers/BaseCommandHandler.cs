using System.Threading.Tasks;
using Qooba.Framework.Cqrs.Abstractions;
using Qooba.Framework.UnitOfWork.Abstractions;

namespace Qooba.Framework.Cqrs.CommandHandlers
{
    public abstract class BaseCommandHandler<TCommand> : BaseHandler, ICommandHandler<TCommand>
        where TCommand : class, ICommand 
    {
        public BaseCommandHandler(IRepositoryCommands<TCommand> repository)
        {
            this.Repository = repository;
        }

        public IRepositoryCommands<TCommand> Repository { get; private set; }

        public abstract Task<CommandResult> Execute(TCommand command);

        protected CommandResult CreateSuccessResult()
        {
            return this.CreateSuccessResult(null);
        }

        protected CommandResult CreateSuccessResult(object data)
        {
            return new CommandResult
            {
                Data = data,
                Success = true
            };
        }

        protected CommandResult CreateFailureResult(string message)
        {
            return new CommandResult
            {
                Message = message
            };
        }
    }
}
