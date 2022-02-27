using FristList.Service.Data.Models.Account;

namespace FristList.Service.Data.Models.Activities;

public class Activity
{
    public Guid Id { get; set; }
    public IList<ActivityCategory> Categories { get; set; } = new List<ActivityCategory>();
    public DateTimeOffset BeginAt { get; set; }
    public DateTimeOffset EndAt { get; set; }
    public User User { get; set; }
}