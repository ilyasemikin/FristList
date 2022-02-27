using FristList.ConsoleClient.Commands;

namespace FristList.ConsoleClient;

public interface IParametersReader
{
    Parameters? Read();
}