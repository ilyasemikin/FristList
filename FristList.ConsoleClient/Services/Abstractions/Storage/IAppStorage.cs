namespace FristList.ConsoleClient.Services.Abstractions.Storage;

public interface IAppStorage
{
    void Set<T>(GenericStorageVariable<T> variable, T? value);
    T? Get<T>(GenericStorageVariable<T> variable);
}