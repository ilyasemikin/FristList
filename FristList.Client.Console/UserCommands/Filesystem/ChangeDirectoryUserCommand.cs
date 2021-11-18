using System;
using System.IO;
using System.Threading.Tasks;
using FristList.Client.Console.Application;
using FristList.Client.Console.Services;

namespace FristList.Client.Console.UserCommands.Filesystem
{
    public class ChangeDirectoryUserCommand : UserCommandBase
    {
        private readonly IMessageWriter _messageWriter;
        private readonly CommandParameters _parameters;
        
        public ChangeDirectoryUserCommand(IMessageWriter messageWriter, CommandParameters parameters) 
            : base("cd")
        {
            _messageWriter = messageWriter;
            _parameters = parameters;
        }

        public override Task<UserCommandExecutionResult> ExecuteAsync()
        {
            if (_parameters.Count == 0)
            {
                _messageWriter.WriteError("Expect 1 parameter");
                return Task.FromResult(DoNothing());
            }

            try
            {
                var toPath = _parameters.Parameters[0].Value;
                if (!Path.IsPathRooted(toPath))
                    toPath = Path.Join(Directory.GetCurrentDirectory(), toPath);

                Directory.SetCurrentDirectory(toPath!);
            }
            catch (Exception e)
            {
                _messageWriter.WriteError(e.Message);
            }

            return Task.FromResult(DoNothing());
        }
    }
}