namespace FristList.Data.Responses;

public class DataResponse<T> : IResponse
{
    public string Message { get; init; }
    public DateTime Time { get; }
    public bool IsSuccess { get; }

    public T Data { get; init; }
    
    public DataResponse(T data)
    {
        Message = string.Empty;
        Time = DateTime.UtcNow;
        IsSuccess = true;

        Data = data;
    }
}