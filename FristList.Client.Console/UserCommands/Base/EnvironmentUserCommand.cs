using System;
using System.Globalization;
using System.Threading.Tasks;
using FristList.Client.Console.Message.Json;
using FristList.Client.Console.Services;

namespace FristList.Client.Console.UserCommands.Base
{
    public class EnvironmentUserCommand : UserCommandBase
    {
        private readonly IMessageWriter _messageWriter;
        
        public EnvironmentUserCommand(IMessageWriter messageWriter) 
            : base("env")
        {
            _messageWriter = messageWriter;
        }

        public override Task<UserCommandExecutionResult> ExecuteAsync()
        {
            var environment = new
            {
                Culture = CultureInfo.CurrentCulture.Name,
                Directory = Environment.CurrentDirectory
            };

            var message = new JsonMessageBuilder().Build(environment);
            _messageWriter.WriteMessage(message);
            
            return Task.FromResult(DoNothing());
        }
    }
}