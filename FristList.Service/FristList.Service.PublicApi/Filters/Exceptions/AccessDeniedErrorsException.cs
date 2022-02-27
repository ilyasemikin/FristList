namespace FristList.Service.PublicApi.Filters.Exceptions;

public class AccessDeniedErrorsException : Exception
{
    public IList<Exception> Errors { get; }

    public AccessDeniedErrorsException(IList<Exception> errors)
    {
        Errors = errors;
    }
}