namespace FristList.Data.Models;

public class RunningAction
{
    public int Id { get; set; }
    
    public int UserId { get; set; }
    public AppUser? User { get; set; }
    
    public int TaskId { get; set; }
    public Task? Task { get; set; }
    
    public List<int> CategoryIds { get; set; }
    public List<Category> Categories { get; set; }

    public RunningAction()
    {
        CategoryIds = new List<int>();
        Categories = new List<Category>();
    }
}