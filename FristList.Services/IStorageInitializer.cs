using System.Threading.Tasks;

namespace FristList.Services
{
    public interface IStorageInitializer
    {
        public Task InitializeAsync();
    }
}