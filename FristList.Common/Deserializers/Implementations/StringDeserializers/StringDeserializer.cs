using JetBrains.Annotations;

namespace FristList.Common.Deserializers.Implementations.StringDeserializers;

[UsedImplicitly]
public class StringDeserializer : IStringDeserializer
{
    public Type DeserializeType => typeof(string);
    
    public object? Deserialize(string input)
    {
        return input;
    }
}