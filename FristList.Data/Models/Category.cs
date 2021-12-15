namespace FristList.Data.Models;

public class Category
{
    public int Id { get; set; }
    
    public int UserId { get; set; }
    public AppUser? User { get; set; }
    
    public string Name { get; set; }
}