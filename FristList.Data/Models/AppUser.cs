using FristList.Data.Models.Base;

namespace FristList.Data.Models;

public class AppUser : ModelObjectBase
{
    public int Id { get; set; }
    public string UserName { get; set; }
    public string NormalizedUserName { get; set; }
    public string Email { get; set; }
    public string NormalizedEmail { get; set; }
    public bool EmailConfirmed { get; set; }
    public string PhoneNumber { get; set; }
    public bool PhoneNumberConfirmed { get; set; }
    public string PasswordHash { get; set; }
    public bool TwoFactorEnable { get; set; }
}