using System;

namespace FristList.Dto.Responses.Base
{
    public class DataResponse<T> : IResponse
    {
        public T Data { get; init; }
        public string Message { get; init; }
        public DateTime Time { get; init; }
        public bool IsSuccess { get; init; }

        public DataResponse(T data)
        {
            Data = data;
            Message = string.Empty;
            Time = DateTime.UtcNow;
            IsSuccess = true;
        }
    }
}