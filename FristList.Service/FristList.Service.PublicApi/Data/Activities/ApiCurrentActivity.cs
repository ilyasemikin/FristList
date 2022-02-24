using FristList.Service.PublicApi.Data.Categories;

namespace FristList.Service.PublicApi.Data.Activities;

public class ApiCurrentActivity
{
    public DateTimeOffset BeginAt { get; set; }
    public IList<ApiCategory> Categories { get; set; } = new List<ApiCategory>();
}