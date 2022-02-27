using FristList.Models.Base;

namespace FristList.Models;

public class RefreshToken : ModelObjectBase
{
    public int Id { get; set; }
    public string Token { get; set; }
    public DateTime Expires { get; set; }
    
    public int UserId { get; set; }
    public AppUser? User { get; set; }

    public RefreshToken()
    {
        Token = string.Empty;
    }
}