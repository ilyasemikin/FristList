namespace FristList.Service.PublicApi.Deserializers;

public interface IStringDeserializer
{
    Type DeserializeType { get; }
    object? Deserialize(string input);
}