using FristList.Service.Data.Models.Activities;

namespace FristList.Service.Data.Models.Categories.Base;

public class BaseCategory
{
    private IList<CurrentActivity> CurrentActivities { get; set; } = new List<CurrentActivity>();
    private IList<ActivityCategory> Activities { get; set; } = new List<ActivityCategory>();

    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
}