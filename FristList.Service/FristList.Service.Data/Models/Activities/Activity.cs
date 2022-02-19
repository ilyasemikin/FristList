using FristList.Service.Data.Models.Categories.Base;

namespace FristList.Service.Data.Models.Activities;

public class Activity
{
    public Guid Id { get; set; }
    public IList<ActivityCategory> Categories { get; set; } = new List<ActivityCategory>();
    public DateTimeOffset BeginAt { get; set; }
    public DateTimeOffset EndAt { get; set; }
}