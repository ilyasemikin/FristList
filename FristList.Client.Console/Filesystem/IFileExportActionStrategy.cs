using System.Collections.Generic;
using FristList.Dto;

namespace FristList.Client.Console.Filesystem
{
    public interface IFileExportActionStrategy
    {
        void Export(IEnumerable<Action> actions, string path);
    }
}