using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace FristList.Client.Console.Services
{
    public class ApplicationInitializer
    {
        private void InitJwtProvider(string path)
        {
            if (!File.Exists(path))
                return;
        }
        
        public Task InitializeAsync()
        {
            return Task.CompletedTask;
        }
    }
}