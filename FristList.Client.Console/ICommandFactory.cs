using FristList.Client.Console.Commands.Base;

namespace FristList.Client.Console;

public interface ICommandFactory
{
    ICommand CreateCommand(CommandRequest request);
}