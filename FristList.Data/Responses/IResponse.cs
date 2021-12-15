using System.Net;

namespace FristList.Data.Responses;

public interface IResponse
{
    string Message { get; }
    DateTime Time { get; }
    bool IsSuccess { get; }
}