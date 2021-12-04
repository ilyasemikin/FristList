using FristList.Client.Console.Services;
using FristList.Client.Console.UserCommands;
using FristList.Client.Console.UserCommands.Base;

namespace FristList.Client.Console.Application.Chains
{
    public class UnknownCommandHandler : CommandHandlerBase
    {
        private readonly IMessageWriter _messageWriter;
        
        public UnknownCommandHandler(IMessageWriter messageWriter)
        {
            _messageWriter = messageWriter;
        }

        public override UserCommandBase GetCommand(CommandContext context)
            => new UnknownUserCommandBase(_messageWriter, context.Command);
    }
}