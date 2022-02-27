namespace FristList.Common.Deserializers;

public interface IStringDeserializer
{
    Type DeserializeType { get; }
    object? Deserialize(string input);
}