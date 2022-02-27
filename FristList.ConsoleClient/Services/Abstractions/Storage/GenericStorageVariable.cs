namespace FristList.ConsoleClient.Services.Abstractions.Storage;

public record GenericStorageVariable<T>(string Name) : StorageVariable(Name);