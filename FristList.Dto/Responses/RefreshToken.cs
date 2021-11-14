using FristList.Dto.Responses.Base;

namespace FristList.Dto.Responses
{
    public class RefreshToken : DtoObjectBase
    {
        public string TokenValue { get; init; }
        public string RefreshTokenValue { get; init; }
    }
}