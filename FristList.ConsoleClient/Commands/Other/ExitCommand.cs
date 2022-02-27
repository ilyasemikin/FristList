using FristList.ConsoleClient.CommandModels;
using FristList.ConsoleClient.Exceptions;

namespace FristList.ConsoleClient.Commands.Other;

public class ExitCommand : ICommand<EmptyModel>
{
    public Task ExecuteAsync()
    {
        throw new ExecutionAbortedException();
    }
}