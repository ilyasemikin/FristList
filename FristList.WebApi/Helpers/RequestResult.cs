using System;
using System.Collections.Generic;

namespace FristList.WebApi.Helpers;

public class RequestResult<T>
{
    public T Data { get; }
    public bool IsSuccess { get; }
    public bool IsEmpty { get; init; }
    public IEnumerable<string> Errors { get; init; }

    private RequestResult(T data, bool isSuccess)
    {
        Data = data;
        IsSuccess = isSuccess;
        IsEmpty = false;
        Errors = Array.Empty<string>();
    }

    public static RequestResult<T> Success(T data)
        => new(data, true);

    public static RequestResult<T> Empty()
        => new(default!, true)
        {
            IsEmpty = true
        };

    public static RequestResult<T> Failed(params string[] errors)
        => new(default!, false)
        {
            Errors = errors
        };
}