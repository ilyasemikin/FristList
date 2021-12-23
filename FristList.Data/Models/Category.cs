using FristList.Data.Models.Base;

namespace FristList.Data.Models;

public class Category : ModelObjectBase
{
    public int Id { get; set; }
    
    public int UserId { get; set; }
    public AppUser? User { get; set; }
    
    public string Name { get; set; }

    public Category()
    {
        Name = string.Empty;
    }
}