namespace FristList.Service.PublicApi.Context;

public class Variable
{
    public string Name { get; }
    public Type Type { get; }
    
    public Variable(string name, Type type)
    {
        Name = name;
        Type = type;
    }
}