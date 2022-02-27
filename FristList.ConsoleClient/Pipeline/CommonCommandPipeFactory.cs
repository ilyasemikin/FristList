using FristList.ConsoleClient.CommandModels;
using FristList.ConsoleClient.Commands;
using FristList.ConsoleClient.Commands.Other;

namespace FristList.ConsoleClient.Pipeline;

public class CommonCommandPipeFactory : ICommandPipeFactory
{
    private readonly ICommandPipeFactory _next;

    public CommonCommandPipeFactory(ICommandPipeFactory next)
    {
        _next = next;
    }

    public ICommand<CommandModelBase> GetCommand(Parameters parameters)
    {
        return parameters.CommandName switch
        {
            "exit" => new ExitCommand(),
            _ => _next.GetCommand(parameters)
        };
    }
}