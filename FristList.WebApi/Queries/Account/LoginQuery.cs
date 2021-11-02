using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FristList.WebApi.Queries.Account
{
    public class LoginQuery
    {
        [Required]
        public string Login { get; set; }
        
        [Required]
        [PasswordPropertyText]
        public string Password { get; set; }
    }
}