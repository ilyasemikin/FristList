using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FristList.Client.Console.Application;
using FristList.Client.Console.Message.Console;
using FristList.Client.Console.Services;

namespace FristList.Client.Console.UserCommands.Base
{
    public class AvailableCommandsUserCommand : UserCommandBase
    {
        private readonly IEnumerable<string> _commands;
        private readonly IMessageWriter _messageWriter;
        private readonly CommandParameters _parameters; 

        public AvailableCommandsUserCommand(IEnumerable<string> commands, IMessageWriter messageWriter, CommandParameters parameters)
            : base("commands")
        {
            _commands = commands;
            _messageWriter = messageWriter;
            _parameters = parameters;
        }

        public override Task<UserCommandExecutionResult> ExecuteAsync()
        {
            var commands = _commands.OrderBy(e => e);
            var message = new ColoredJsonMessageBuilder().Build(commands);
            _messageWriter.WriteMessage(message);
            return Task.FromResult(DoNothing());
        }
    }
}