using FristList.Models.Base;

namespace FristList.Models;

public class Category : ModelObjectBase
{
    public int Id { get; set; }

    public string Name { get; set; }
    
    public int UserId { get; set; }
    public AppUser? User { get; set; }

    public Category()
    {
        Name = string.Empty;
    }
}