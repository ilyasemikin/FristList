using FristList.Models.Base;

namespace FristList.Models;

public class AppUser : ModelObjectBase, IEquatable<AppUser>
{
    public int Id { get; set; }
    public string UserName { get; set; }
    public string NormalizedUserName { get; set; }
    public string? Email { get; set; }
    public string? NormalizedEmail { get; set; }
    public bool EmailConfirmed { get; set; }
    public string? PhoneNumber { get; set; }
    public bool PhoneNumberConfirmed { get; set; }
    public string PasswordHash { get; set; }
    public bool TwoFactorEnable { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime EditedAt { get; set; }

    public AppUser()
    {
        UserName = NormalizedUserName = PasswordHash = string.Empty;
    }

    public bool Equals(AppUser? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Id == other.Id && NormalizedUserName == other.NormalizedUserName;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((AppUser)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id, NormalizedUserName);
    }
}