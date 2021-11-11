using FristList.Client.Console.Application;
using FristList.Client.Console.UserCommands;

namespace FristList.Client.Console.Services
{
    public interface ICommandParametersReader
    {
        CommandParameters Read();
    }
}