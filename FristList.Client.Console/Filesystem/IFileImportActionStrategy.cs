using System.Collections.Generic;
using FristList.Dto;

namespace FristList.Client.Console.Filesystem
{
    public interface IFileImportActionStrategy
    {
        IEnumerable<Action> Import(string path);
    }
}