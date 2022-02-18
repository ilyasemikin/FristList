namespace FristList.Service.PublicApi.Responses;

public class ApiError
{
    public IEnumerable<string> Errors { get; set; } = Array.Empty<string>();
}