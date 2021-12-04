using System.Threading.Tasks;
using FristList.Client.Console.Services;

namespace FristList.Client.Console.UserCommands.Base
{
    public sealed class UnknownUserCommandBase : UserCommandBase
    {
        private readonly IMessageWriter _messageWriter;
        
        public string CommandName { get; }
        
        public UnknownUserCommandBase(IMessageWriter messageWriter, string commandName)
            : base(string.Empty)
        {
            _messageWriter = messageWriter;
            CommandName = commandName;
        }
        
        public override Task<UserCommandExecutionResult> ExecuteAsync()
        {
            _messageWriter.WriteError($"Unknown command {CommandName}");

            return Task.FromResult(DoNothing());
        }
    }
}