namespace FristList.Service.Data.Models.Account;

public class RefreshToken
{
    public Guid Id { get; set; }
    public string Token { get; set; } = null!;
    public DateTimeOffset ExpiresAt { get; set; }
    public User User { get; set; } = null!;
}