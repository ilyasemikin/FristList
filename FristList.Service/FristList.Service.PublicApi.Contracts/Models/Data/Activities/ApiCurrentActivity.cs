using FristList.Service.PublicApi.Contracts.Models.Data.Categories;

namespace FristList.Service.PublicApi.Contracts.Models.Data.Activities;

public class ApiCurrentActivity
{
    public DateTimeOffset BeginAt { get; set; }
    public IList<ApiCategory> Categories { get; set; } = new List<ApiCategory>();
}