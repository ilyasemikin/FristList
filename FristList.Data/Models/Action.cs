namespace FristList.Data.Models;

public class Action
{
    public int Id { get; set; }
    
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    
    public string Description { get; set; }
    
    public int UserId { get; set; }
    public AppUser? User { get; set; }
    
    public IList<int> CategoryIds { get; set; }
    public IList<Category> Categories { get; set; }

    public Action()
    {
        Description = string.Empty;
        
        CategoryIds = new List<int>();
        Categories = new List<Category>();
    }
}