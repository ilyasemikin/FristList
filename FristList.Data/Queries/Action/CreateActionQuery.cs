using System.ComponentModel.DataAnnotations;
using FristList.Data.Validations;

namespace FristList.Data.Queries.Action;

public class CreateActionQuery
{
    [Required]
    [LessThan(nameof(EndTime))]
    public DateTime? StartTime { get; init; }
    [Required]
    public DateTime? EndTime { get; init; }
    
    public string? Description { get; init; }
    public IReadOnlyList<int> CategoryIds { get; init; }

    public CreateActionQuery()
    {
        CategoryIds = Array.Empty<int>();
    }
}