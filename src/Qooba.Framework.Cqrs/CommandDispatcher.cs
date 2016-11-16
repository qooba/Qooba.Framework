using Qooba.Framework.Cqrs.Abstractions;
using Qooba.Framework.DependencyInjection.Abstractions;
using System.Threading.Tasks;
using System;

namespace Qooba.Framework.Cqrs
{
    public class CommandDispatcher : ICommandDispatcher
    {
        private readonly IFactory factory;

        public CommandDispatcher(IFactory factory)
        {
            this.factory = factory;
        }
        
        public async Task<CommandResult> Dispatch<TParameter>(TParameter command) where TParameter : ICommand
        {
            return await this.factory.Create<ICommandHandler<TParameter>>().Execute(command);
        }
    }
}
