using FristList.Service.PublicApi.Data.Categories;

namespace FristList.Service.PublicApi.Models.Activities;

public class AddActivityModel
{
    public IList<Guid> CategoryIds { get; set; } = new List<Guid>();
    
    public DateTimeOffset BeginAt { get; set; }
    public DateTimeOffset EndAt { get; set; }
}