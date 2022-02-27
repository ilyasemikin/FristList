using FristList.ConsoleClient.CommandModels;
using FristList.ConsoleClient.Commands.Other.Exceptions;

namespace FristList.ConsoleClient.Commands.Other;

public class UnknownCommand : ICommand<EmptyModel>
{
    private readonly string _commandName;

    public UnknownCommand(string commandName)
    {
        this._commandName = commandName;
    }

    public Task ExecuteAsync()
    {
        throw new UnknownCommandException(_commandName);
    }
}