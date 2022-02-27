using System.Threading.Tasks;
using FristList.Client.Console.Commands.Base;

namespace FristList.Client.Console.Commands;

public class ExitCommand : ICommand
{
    public Task<CommandResult> RunAsync()
        => Task.FromResult(new CommandResult
        {
            ShouldQuit = true
        });
}