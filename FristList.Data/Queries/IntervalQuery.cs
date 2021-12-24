namespace FristList.Data.Queries;

public class IntervalQuery
{
    public DateTime From { get; init; }
    public DateTime To { get; init; }

    public IntervalQuery()
    {
        From = DateTime.UtcNow - TimeSpan.FromDays(365);
        To = DateTime.UtcNow;
    }
}