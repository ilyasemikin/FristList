using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FristList.Data.Queries.Account;

public class RegisterQuery
{
    [Required]
    public string UserName { get; init; }
    
    [Required]
    [PasswordPropertyText]
    public string Password { get; init; }
    
    [Required]
    [EmailAddress]
    public string Email { get; init; }
    
    [Required]
    [PasswordPropertyText]
    [Compare(nameof(Password))]
    public string ConfirmPassword { get; init; }

    public RegisterQuery()
    {
        UserName = Password = ConfirmPassword = string.Empty;
    }
}