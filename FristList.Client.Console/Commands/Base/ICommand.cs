using System.Threading.Tasks;

namespace FristList.Client.Console.Commands.Base;

public interface ICommand
{
    Task<CommandResult> RunAsync();
}