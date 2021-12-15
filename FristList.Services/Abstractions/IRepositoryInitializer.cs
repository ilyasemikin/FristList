using System.Threading.Tasks;

namespace FristList.Services.Abstractions;

public interface IRepositoryInitializer
{
    Task InitializeAsync();
}