using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FristList.Dto.Queries.Account
{
    public class RegisterAccountQuery
    {
        [Required]
        public string UserName { get; set; }
        
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        
        [Required]
        [PasswordPropertyText]
        public string Password { get; set; }
        
        [Required]
        [PasswordPropertyText]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
    }
}