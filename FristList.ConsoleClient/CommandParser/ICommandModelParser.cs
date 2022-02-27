using FristList.ConsoleClient.CommandModels;
using FristList.ConsoleClient.Commands;

namespace FristList.ConsoleClient.CommandParser;

public interface ICommandModelParser
{
    TCommandModel Parse<TCommandModel>(Parameters parameters) where TCommandModel : CommandModelBase;
}