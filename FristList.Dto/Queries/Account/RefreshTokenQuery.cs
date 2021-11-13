using System.ComponentModel.DataAnnotations;

namespace FristList.Dto.Queries.Account
{
    public class RefreshTokenQuery
    {
        [Required]
        public string Token { get; set; }
    }
}