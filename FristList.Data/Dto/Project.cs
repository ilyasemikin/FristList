using FristList.Data.Dto.Base;

namespace FristList.Data.Dto;

public class Project : DtoObjectBase
{
    public int Id { get; init; }
    public string Name { get; init; }
    public string? Description { get; set; }
    public bool IsCompleted { get; init; }

    public Project()
    {
        Name = string.Empty;
    }
}