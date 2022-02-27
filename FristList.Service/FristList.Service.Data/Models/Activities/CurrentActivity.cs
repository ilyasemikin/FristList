using FristList.Service.Data.Models.Account;
using FristList.Service.Data.Models.Categories.Base;

namespace FristList.Service.Data.Models.Activities;

public class CurrentActivity
{
    public Guid UserId { get; set; }
    public User User { get; set; }
    public DateTimeOffset BeginAt { get; set; }
    public IList<BaseCategory> Categories { get; set; } = new List<BaseCategory>();
}