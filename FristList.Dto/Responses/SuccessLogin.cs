using FristList.Dto.Responses.Base;

namespace FristList.Dto.Responses
{
    public class SuccessLogin : DtoObjectBase
    {
        public string Token { get; init; }
        public string RefreshToken { get; init; }
    }
}
