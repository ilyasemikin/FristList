using FristList.Client.Console.Message;

namespace FristList.Client.Console.Services
{
    public interface IMessageWriter
    {
        void WriteText(string text);

        void WriteMessage(MessageNodeBase messageNode);
        void WriteMessage(string message);
        void WriteWarning(string warning);
        void WriteError(string error);
    }
}