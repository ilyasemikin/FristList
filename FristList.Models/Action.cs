using FristList.Models.Base;

namespace FristList.Models;

public class Action : ModelObjectBase
{
    public int Id { get; set; }
    
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    
    public string? Description { get; set; }
    
    public int UserId { get; set; }
    public AppUser? User { get; set; }
    
    public IList<int> CategoryIds { get; set; }
    public IList<Category> Categories { get; set; }

    public Action()
    {
        CategoryIds = new List<int>();
        Categories = new List<Category>();
    }
}