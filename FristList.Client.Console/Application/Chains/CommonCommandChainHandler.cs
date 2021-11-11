using System.Collections.Generic;
using System.Linq;
using FristList.Client.Console.Services;
using FristList.Client.Console.UserCommands;
using FristList.Client.Console.UserCommands.Base;

namespace FristList.Client.Console.Application.Chains
{
    public class CommonCommandChainHandler : CommandChainHandlerBase
    {
        private readonly IMessageWriter _messageWriter;
        
        private readonly HashSet<string> _commands;

        public CommonCommandChainHandler(IMessageWriter messageWriter)
        {
            _messageWriter = messageWriter;

            _commands = new HashSet<string>
            {
                "exit",
                "commands"
            };
        }

        public override UserCommandBase GetCommand(CommandContext context)
        {
            UserCommandBase command = context.Command switch
            {
                "exit" => new ExitUserCommand(),
                "commands" => new AvailableCommandsUserCommand(GetCommands(), _messageWriter, context.Parameters),
                _ => null
            };

            return command ?? Next.GetCommand(context);
        }

        public override bool ContainsCommand(string command)
            => _commands.Contains(command) || base.ContainsCommand(command);

        public override IEnumerable<string> GetCommands()
            => _commands.Concat(base.GetCommands());
    }
}