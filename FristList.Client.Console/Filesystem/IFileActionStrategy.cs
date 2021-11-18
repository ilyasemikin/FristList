namespace FristList.Client.Console.Filesystem
{
    public interface IFileActionStrategyFactory
    {
        IFileImportActionStrategy CreateImportActionStrategy(string name);
        IFileExportActionStrategy CreateExportActionStrategy(string name);
    }
}