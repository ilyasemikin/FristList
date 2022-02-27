using FristList.ConsoleClient.CommandModels;
using FristList.ConsoleClient.Commands;

namespace FristList.ConsoleClient.Pipeline;

public interface ICommandPipeFactory
{
    ICommand<CommandModelBase> GetCommand(Parameters parameters);
}