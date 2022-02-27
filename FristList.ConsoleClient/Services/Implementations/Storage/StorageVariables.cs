using FristList.ConsoleClient.Services.Abstractions.Storage;

namespace FristList.ConsoleClient.Services.Implementations.Storage;

public static class StorageVariables
{
    public static readonly GenericStorageVariable<AuthorizeSettings> Authorization = new(nameof(Authorization));
}