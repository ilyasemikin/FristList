using FristList.Service.PublicApi.Deserializers.Implementations.StringDeserializers;

namespace FristList.Service.PublicApi.Deserializers;

public static class StringDeserializers
{
    private static readonly IReadOnlyDictionary<Type, IStringDeserializer> Deserializers =
        new Dictionary<Type, IStringDeserializer>
        {
            [typeof(Guid)] = new GuidDeserializer(),
            [typeof(Guid?)] = new GuidDeserializer(),
            [typeof(string)] = new StringDeserializer()
        };

    public static object? Deserialize(string input, Type returnType)
    {
        if (!Deserializers.TryGetValue(returnType, out var deserializer))
            return null;
        return deserializer.Deserialize(input);
    }

    public static T? Deserialize<T>(string input)
    {
        var deserialized = Deserialize(input, typeof(T));
        if (deserialized is null)
            return default;
        return (T)deserialized;
    }
}