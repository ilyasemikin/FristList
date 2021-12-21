using FristList.Data.Dto.Base;

namespace FristList.Data.Dto;

public class AppUser : DtoObjectBase
{
    public int Id { get; init; }
    public string UserName { get; init; }
    public string Email { get; init; }

    public AppUser()
    {
        UserName = string.Empty;
        Email = string.Empty;
    }
}