using System;
using System.Threading.Tasks;
using FristList.Client.Console.Application.Chains;
using FristList.Client.Console.Services;
using FristList.Client.Console.UserCommands;

namespace FristList.Client.Console.Application
{
    public class Executor
    {
        private readonly ICommandParametersReader _commandParametersReader;
        private readonly IMessageWriter _messageWriter;
        private readonly CommandHandlerBase _commandHandler;

        public Executor(ICommandParametersReader commandParametersReader, IMessageWriter messageWriter, CommandHandlerBase commandHandler)
        {
            _messageWriter = messageWriter;
            _commandHandler = commandHandler;
            _commandParametersReader = commandParametersReader;
        }

        public async Task RunAsync()
        {
            while (true)
            {
                try
                {
                    var parameters = _commandParametersReader.Read();

                    var context = new CommandContext(parameters);

                    var command = _commandHandler.GetCommand(context);
                    var result = await command.ExecuteAsync();
                    
                    if (result.Exit)
                        break;
                }
                catch (Exception e)
                {
                    _messageWriter.WriteError(e.Message);

                    while (e.InnerException is not null)
                    {
                        _messageWriter.WriteError($"Inner error: {e.InnerException.Message}");
                        e = e.InnerException;
                    }
                }
            }
        }
    }
}