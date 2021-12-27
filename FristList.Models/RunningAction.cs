using FristList.Models.Base;
using Task = FristList.Models.Task;

namespace FristList.Models;

public class RunningAction : ModelObjectBase
{
    public int Id { get; set; }
    
    public DateTime StartTime { get; init; }

    public int UserId { get; set; }
    public AppUser? User { get; set; }
    
    public int? TaskId { get; set; }
    public Task? Task { get; set; }
    
    public IList<int> CategoryIds { get; set; }
    public IList<Category> Categories { get; set; }

    public RunningAction()
    {
        CategoryIds = new List<int>();
        Categories = new List<Category>();
    }
}