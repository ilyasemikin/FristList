using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace FristList.Data.Queries.Account;

public class RefreshTokenQuery
{
    [Required]
    public string Token { get; init; }
}