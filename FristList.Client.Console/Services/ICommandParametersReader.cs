using FristList.Client.Console.Application;

namespace FristList.Client.Console.Services
{
    public interface ICommandParametersReader
    {
        CommandParameters Read();
    }
}