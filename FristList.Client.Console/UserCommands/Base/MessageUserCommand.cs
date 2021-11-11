using System.Collections.Generic;
using System.Threading.Tasks;
using FristList.Client.Console.Services;
using Microsoft.CSharp.RuntimeBinder;

namespace FristList.Client.Console.UserCommands.Base
{
    public class MessageUserCommand : UserCommandBase
    {
        private readonly IMessageWriter _messageWriter;
        
        public enum MessageType
        {
            Message,
            Warning,
            Error
        };

        public string Message { get; }
        public MessageType Type { get; }
        
        public MessageUserCommand(IMessageWriter messageWriter, string message, MessageType type = MessageType.Message)
            : base("message")
        {
            _messageWriter = messageWriter;
            
            Message = message;
            Type = type;
        }
        
        public override Task<UserCommandExecutionResult> ExecuteAsync()
        {
            switch (Type)
            {
                case MessageType.Error:
                    _messageWriter.WriteError(Message);
                    break;
                case MessageType.Message:
                    _messageWriter.WriteMessage(Message);
                    break;
                case MessageType.Warning:
                    _messageWriter.WriteWarning(Message);
                    break;
                default:
                    break;
            }
            
            return Task.FromResult(DoNothing());
        }
    }
}