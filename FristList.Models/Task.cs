using FristList.Models.Base;

namespace FristList.Models;

public class Task : ModelObjectBase
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool IsCompleted { get; set; }
    
    public int AuthorId { get; set; }
    public AppUser? Author { get; set; }
    
    public IList<int> CategoryIds { get; set; }
    public IList<Category> Categories { get; set; }
    
    public int? ProjectId { get; set; }
    public Project? Project { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime EditedAt { get; set; }

    public Task()
    {
        Name = string.Empty;
        
        CategoryIds = new List<int>();
        Categories = new List<Category>();
    }
}