using System;

namespace FristList.Dto.Responses.Base
{
    public class Response<T> : IResponse<T>
    {
        public T Data { get; init; }
        public string Message { get; init; }
        public DateTime Time { get; init; }
        public bool IsSuccess { get; init; }

        public Response(T data)
        {
            Data = data;
            Message = string.Empty;
            Time = DateTime.UtcNow;
            IsSuccess = true;
        }
    }
}