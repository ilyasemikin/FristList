using FristList.Data.Dto.Base;

namespace FristList.Data.Dto;

public class Task : DtoObjectBase
{
    public int Id { get; init; }
    public string Name { get; init; }
    public bool IsCompleted { get; init; }
    
    public IReadOnlyList<Category> Categories { get; init; }

    public Task()
    {
        Name = string.Empty;
        Categories = Array.Empty<Category>();
    }
}