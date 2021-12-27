using FristList.Models.Base;

namespace FristList.Models;

public class Project : ModelObjectBase
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public bool IsCompleted { get; set; }
    
    public int UserId { get; set; }
    public AppUser? User { get; set; }

    public Project()
    {
        Name = string.Empty;
    }
}