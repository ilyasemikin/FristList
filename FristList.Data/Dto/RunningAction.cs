using FristList.Data.Dto.Base;

namespace FristList.Data.Dto;

public class RunningAction : DtoObjectBase
{
    public DateTime StartTime { get; init; }
    
    public int? TaskId { get; init; }
    public IReadOnlyList<Category> Categories { get; init; }
    
    public RunningAction()
    {
        Categories = Array.Empty<Category>();
    }
}