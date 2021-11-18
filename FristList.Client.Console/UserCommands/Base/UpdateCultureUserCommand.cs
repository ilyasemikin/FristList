using System;
using System.Globalization;
using System.Threading.Tasks;
using FristList.Client.Console.Application;
using FristList.Client.Console.Services;

namespace FristList.Client.Console.UserCommands.Base
{
    public class UpdateCultureUserCommand : UserCommandBase
    {
        private readonly IMessageWriter _messageWriter;
        private readonly CommandParameters _parameters;
        
        public UpdateCultureUserCommand(IMessageWriter messageWriter, CommandParameters parameters) 
            : base("update culture")
        {
            _messageWriter = messageWriter;
            _parameters = parameters;
        }

        public override Task<UserCommandExecutionResult> ExecuteAsync()
        {
            if (_parameters.Count == 0)
                _messageWriter.WriteError("Require 1 parameter");

            var cultureName = _parameters.Parameters[0].Value;

            try
            {
                CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo(cultureName, true);
            }
            catch (Exception e)
            {
                _messageWriter.WriteError(e.Message);
            }

            return Task.FromResult(DoNothing());
        }
    }
}