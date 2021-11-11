using System;

namespace FristList.Dto.Responses
{
    public class Response<T>
    {
        public T Data { get; init; }
        public string Message { get; init; }
        public DateTime Time { get; init; }

        public Response(T data)
        {
            Data = data;
            Message = string.Empty;
            Time = DateTime.UtcNow;
        }
    }
}