using System.Collections.Generic;
using System.Linq;
using FristList.Client.Console.Services;
using FristList.Client.Console.UserCommands;
using FristList.Client.Console.UserCommands.Filesystem;

namespace FristList.Client.Console.Application.Chains
{
    public class FilesystemChainHandler : CommandChainHandlerBase
    {
        private readonly IMessageWriter _messageWriter;
        private readonly HashSet<string> _commands;

        public FilesystemChainHandler(IMessageWriter messageWriter)
        {
            _messageWriter = messageWriter;
            _commands = new HashSet<string>
            {
                "cd"
            };
        }
        
        public override UserCommandBase GetCommand(CommandContext context)
        {
            var command = context.Command switch
            {
                "cd" => new ChangeDirectoryUserCommand(_messageWriter, context.Parameters),
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