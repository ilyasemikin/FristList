using FristList.Models.Base;

namespace FristList.Models;

public class Project : ModelObjectBase
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public bool IsCompleted { get; set; }
    
    public int AuthorId { get; set; }
    public AppUser? Author { get; set; }
    
    public IList<int> AllowUserIds { get; set; }
    public IList<AppUser> AllowUsers { get; set; }

    public Project()
    {
        Name = string.Empty;

        AllowUserIds = new List<int>();
        AllowUsers = new List<AppUser>();
    }
}