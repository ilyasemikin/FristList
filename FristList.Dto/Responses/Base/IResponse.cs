using System;

namespace FristList.Dto.Responses.Base
{
    public interface IResponse
    {
        string Message { get; }
        DateTime Time { get; }
        bool IsSuccess { get; }
    }
}