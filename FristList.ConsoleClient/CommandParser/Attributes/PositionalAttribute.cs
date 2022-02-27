namespace FristList.ConsoleClient.CommandParser.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class PositionalAttribute : Attribute
{
    public int Position { get; }

    public PositionalAttribute(int position)
    {
        if (position < 0)
            throw new ArgumentException("position must be positive", nameof(position));
        Position = position;
    }
}