using System.ComponentModel.DataAnnotations;

namespace FristList.Data.Queries.Account;

public class RefreshTokenQuery
{
    [Required]
    public string Token { get; init; }
}