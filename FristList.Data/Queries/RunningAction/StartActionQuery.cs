namespace FristList.Data.Queries.RunningAction;

public class StartActionQuery
{
    public int? TaskId { get; init; }
    public IReadOnlyList<int>? CategoryIds { get; init; }
}