using JetBrains.Annotations;

namespace FristList.Service.PublicApi.Deserializers.Implementations.StringDeserializers;

[UsedImplicitly]
public class StringDeserializer : IStringDeserializer
{
    public Type DeserializeType => typeof(string);
    
    public object? Deserialize(string input)
    {
        return input;
    }
}