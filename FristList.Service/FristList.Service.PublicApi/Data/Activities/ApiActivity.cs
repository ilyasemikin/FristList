using FristList.Service.PublicApi.Data.Categories;

namespace FristList.Service.PublicApi.Data.Activities;

public class ApiActivity
{
    public Guid Id { get; set; } 
    public DateTimeOffset BeginAt { get; set; }
    public DateTimeOffset EndAt { get; set; }
    public IList<ApiCategory> Categories { get; set; } = null!;
}