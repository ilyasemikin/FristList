using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FristList.Client.Console;

public class CommandExecutor
{
    private readonly Func<CommandRequest?> _commandReader;
    private readonly ICommandFactory _commandFactory;

    public CommandExecutor(Func<CommandRequest?> commandReader, ICommandFactory commandFactory)
    {
        _commandReader = commandReader;
        _commandFactory = commandFactory;
    }
    
    public async Task RunAsync()
    {
        while (true)
        {
            var request = _commandReader();
            if (request is null)
                return;
            
            var command = _commandFactory.CreateCommand(request);
            
            try
            {
                var result = await command.RunAsync();
                
                if (result.ShouldQuit)
                    break;
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.Message);
            }
        }
    }
}