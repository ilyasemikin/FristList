using FristList.Service.Data.Models.Categories.Base;

namespace FristList.Service.Data.Models.Activities;

public class ActivityCategory
{
    public Guid ActivityId { get; set; }
    public Guid CategoryId { get; set; }

    public Activity Activity { get; set; } = null!;
    public BaseCategory Category { get; set; } = null!;
}