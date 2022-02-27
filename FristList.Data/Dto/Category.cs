using FristList.Data.Dto.Base;

namespace FristList.Data.Dto;

public class Category : DtoObjectBase
{
    public int Id { get; init; }
    public string Name { get; init; }

    public Category()
    {
        Name = string.Empty;
    }
}