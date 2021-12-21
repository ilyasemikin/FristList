using System.ComponentModel.DataAnnotations;

namespace FristList.Data.Queries.Task;

public class CreateTaskQuery
{
    [Required]
    public string Name { get; init; }
    public IReadOnlyList<int> CategoryIds { get; init; }

    public CreateTaskQuery()
    {
        Name = string.Empty;
        CategoryIds = Array.Empty<int>();
    }
}