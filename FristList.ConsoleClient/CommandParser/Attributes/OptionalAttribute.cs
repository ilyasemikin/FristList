namespace FristList.ConsoleClient.CommandParser.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class OptionalAttribute : Attribute
{
    public string Name { get; }

    public OptionalAttribute(string name)
    {
        Name = name;
    }
}