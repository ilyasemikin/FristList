using FristList.Data.Models.Base;

namespace FristList.Data.Models;

public class RefreshToken : ModelObjectBase
{
    public int Id { get; set; }
    public string Token { get; set; }
    public DateTime Expires { get; set; }
    
    public int UserId { get; set; }
    public AppUser User { get; set; }
}