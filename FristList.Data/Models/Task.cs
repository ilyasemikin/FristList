using FristList.Data.Models.Base;

namespace FristList.Data.Models;

public class Task : ModelObjectBase
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool IsCompleted { get; set; }
    
    public int UserId { get; set; }
    public AppUser? User { get; set; }
    
    public IList<int> CategoryIds { get; set; }
    public IList<Category> Categories { get; set; }
    
    public int? ProjectId { get; set; }
    public Project? Project { get; set; }

    public Task()
    {
        Name = string.Empty;
        
        CategoryIds = new List<int>();
        Categories = new List<Category>();
    }
}