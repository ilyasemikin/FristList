namespace FristList.ConsoleClient.Services.Responses;

public sealed class ApiResponse<T>
{
    public bool IsSuccess { get; }
    public IReadOnlyList<string> Errors { get; private init; } = Array.Empty<string>();
    public T? Data { get; }

    private ApiResponse(bool isSuccess, T? data = default)
    {
        IsSuccess = isSuccess;
        Data = data;
    }

    public static ApiResponse<T> Success(T data)
    {
        return new ApiResponse<T>(true, data);
    }

    public static ApiResponse<T> Failure(params string[] errors)
    {
        return new ApiResponse<T>(false)
        {
            Errors = errors
        };
    }
}