using System.Threading.Tasks;
using FristList.Client.Console.Commands.Base;

namespace FristList.Client.Console.Commands;

public class NotFoundCommand : ICommand
{
    private readonly CommandRequest _request;

    public NotFoundCommand(CommandRequest request)
    {
        _request = request;
    }

    public Task<CommandResult> RunAsync()
    {
        System.Console.WriteLine($"Command not found \"{_request.Name}\"");
        return Task.FromResult(new CommandResult());
    }
}