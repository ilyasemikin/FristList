using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FristList.Data.Queries.Account;

public class LoginQuery
{
    [Required]
    public string Login { get; init; }
    
    [Required]
    [PasswordPropertyText]
    public string Password { get; init; }
}