namespace FristList.Service.PublicApi.Context;

public class GenericVariable<T> : Variable
{
    public GenericVariable(string name)
        : base(name, typeof(T))
    {
        
    }
}