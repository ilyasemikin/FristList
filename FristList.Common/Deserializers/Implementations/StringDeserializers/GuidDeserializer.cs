using JetBrains.Annotations;

namespace FristList.Common.Deserializers.Implementations.StringDeserializers;

[UsedImplicitly]
public class GuidDeserializer : IStringDeserializer
{
    public Type DeserializeType => typeof(Guid);
    
    public object? Deserialize(string input)
    {
        if (!Guid.TryParse(input, out var guid))
            return null;
        return guid;
    }
}