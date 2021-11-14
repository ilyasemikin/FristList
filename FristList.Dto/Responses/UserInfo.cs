using System;
using FristList.Dto.Responses.Base;

namespace FristList.Dto.Responses
{
    public class UserInfo : DtoObjectBase
    {
        public int Id { get; init; }
        public string UserName { get; init; }
        public string Email { get; init; }
        public DateTime RegistrationTime { get; init; }
    }
}