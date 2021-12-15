namespace FristList.Data.Queries;

public class PagedQuery
{
    public int Page { get; init; }
    public int PageSize { get; init; }

    public PagedQuery()
    {
        Page = 1;
        PageSize = 50;
    }
}