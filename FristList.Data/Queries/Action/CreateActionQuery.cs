namespace FristList.Data.Queries.Action;

public class CreateActionQuery
{
    public DateTime StartTime { get; init; }
    public DateTime EndTime { get; init; }
    public string? Description { get; init; }
    public IReadOnlyList<int> CategoryIds { get; init; }

    public CreateActionQuery()
    {
        CategoryIds = Array.Empty<int>();
    }
}