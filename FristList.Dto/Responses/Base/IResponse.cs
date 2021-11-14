using System;

namespace FristList.Dto.Responses.Base
{
    public interface IResponse<out T>
    {
        T Data { get; }
        string Message { get; }
        DateTime Time { get; }
        bool IsSuccess { get; }
    }
}