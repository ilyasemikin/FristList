namespace FristList.Service.PublicApi.Context;

public static class RequestContextVariables
{
    public static readonly GenericVariable<Guid> UserId = new (nameof(UserId));
}