using FristList.ConsoleClient.CommandModels;

namespace FristList.ConsoleClient.Commands;

public interface ICommand<out TCommandModel>
    where TCommandModel : CommandModelBase
{
    Task ExecuteAsync();
}